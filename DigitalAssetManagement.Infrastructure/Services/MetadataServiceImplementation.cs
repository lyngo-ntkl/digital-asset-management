using AutoMapper;
using DigitalAssetManagement.Application.Common;
using DigitalAssetManagement.Application.Dtos.Requests.Drives;
using DigitalAssetManagement.Application.Dtos.Responses.Drives;
using DigitalAssetManagement.Application.Exceptions;
using DigitalAssetManagement.Application.Repositories;
using DigitalAssetManagement.Application.Services;
using DigitalAssetManagement.Domain.Entities;
using DigitalAssetManagement.Domain.Enums;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq.Expressions;

namespace DigitalAssetManagement.Infrastructure.Services
{
    public class MetadataServiceImplementation : MetadataService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserService _userService;
        private readonly PermissionService _permissionService;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IConfiguration _configuration;

        public MetadataServiceImplementation(UnitOfWork unitOfWork, IMapper mapper, UserService userService, PermissionService permissionService, IBackgroundJobClient backgroundJobClient, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userService = userService;
            _permissionService = permissionService;
            _backgroundJobClient = backgroundJobClient;
            _configuration = configuration;
        }

        public async Task<DriveDetailsResponseDto> Create(DriveRequestDto request)
        {
            User loginUser = await _userService.GetLoginUserAsync();
            var drive = new Drive {
                DriveName = request.DriveName,
                OwnerId = loginUser.Id!.Value
            };

            drive = await _unitOfWork.DriveRepository.InsertAsync(drive);
            await _unitOfWork.SaveAsync();

            drive.HierarchicalPath = new Microsoft.EntityFrameworkCore.LTree($"{loginUser.Id}.{drive.Id}");
            _unitOfWork.DriveRepository.Update(drive);
            await _unitOfWork.SaveAsync();

            await _permissionService.CreatePermission(loginUser.Id!.Value, drive.Id!.Value, Role.Admin, typeof(Drive));

            return _mapper.Map<DriveDetailsResponseDto>(drive);
        }

        public async Task Delete(int id)
        {
            if (!await _permissionService.HasPermissionLoginUser(Role.Admin, id, typeof(Drive)))
            {
                throw new ForbiddenException(ExceptionMessage.UnallowedModification);
            }

            var drive = await _unitOfWork.DriveRepository.GetByIdAsync(id);
            if (drive == null)
            {
                throw new NotFoundException(ExceptionMessage.DriveNotFound);
            }

            var user = await _userService.GetLoginUserAsync();
            if (drive.OwnerId != user.Id)
            {
                throw new ForbiddenException(ExceptionMessage.UnallowedModification);
            }

            _unitOfWork.DriveRepository.Delete(drive);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteDrive(int id)
        {
            var drive = await _unitOfWork.DriveRepository.GetByIdAsync(id);
            if (drive == null)
            {
                throw new NotFoundException(ExceptionMessage.DriveNotFound);
            }
            
            _unitOfWork.DriveRepository.Delete(drive);
            await _unitOfWork.SaveAsync();
        }

        public async Task<DriveDetailsResponseDto> GetById(int id)
        {
            if (! await _permissionService.HasPermissionLoginUser(Role.Reader, id, typeof(Drive)))
            {
                throw new ForbiddenException(ExceptionMessage.UnallowedAccess);
            }

            var drive = await _unitOfWork.DriveRepository.GetByIdAsync(id);
            if (drive == null)
            {
                throw new NotFoundException(ExceptionMessage.DriveNotFound);
            }

            var user = await _userService.GetLoginUserAsync();
            if (drive.OwnerId != user.Id)
            {
                throw new ForbiddenException(ExceptionMessage.UnallowedAccess);
            }

            return _mapper.Map<DriveDetailsResponseDto>(drive);
        }

        public async Task<List<DriveResponseDto>> GetDriveOwnedByLoginUser(string? name)
        {
            User user = await _userService.GetLoginUserAsync();

            var parameter = Expression.Parameter(typeof(Drive));
            Expression expression = Expression.Equal(
                Expression.Constant(user.Id),
                Expression.Property(parameter, nameof(Drive.OwnerId))
            );

            if (!string.IsNullOrWhiteSpace(name))
            {
                expression = Expression.And(
                    expression,
                    Expression.Call(
                        Expression.Call(
                            Expression.Property(parameter, nameof(Drive.DriveName)),
                            typeof(string).GetMethod("ToLower", Type.EmptyTypes) ?? throw new InvalidOperationException("Method string.Contains is invalid or deprecated")
                        ), 
                        typeof(string).GetMethod("Contains", new[] { typeof(string) }) ?? throw new InvalidOperationException("Method string.Contains is invalid or deprecated"),
                        Expression.Constant(name.ToLower())
                    )
                );
            }

            var drives = await _unitOfWork.DriveRepository.GetAllAsync(filter: Expression.Lambda<Func<Drive, bool>>(expression, parameter));
            return _mapper.Map<List<DriveResponseDto>>(drives);
        }

        public async Task MoveToTrash(int id)
        {
            if (!await _permissionService.HasPermissionLoginUser(Role.Contributor, id, typeof(Drive)))
            {
                throw new ForbiddenException(ExceptionMessage.UnallowedModification);
            }

            var drive = await _unitOfWork.DriveRepository.GetByIdAsync(id);
            if (drive == null)
            {
                throw new NotFoundException(ExceptionMessage.DriveNotFound);
            }

            var user = await _userService.GetLoginUserAsync();
            if (drive.OwnerId != user.Id)
            {
                throw new ForbiddenException(ExceptionMessage.UnallowedModification);
            }

            drive.IsDeleted = true;
            _unitOfWork.DriveRepository.Update(drive);
            await _unitOfWork.FolderRepository.BatchUpdateAsync(folder => folder.SetProperty(f => f.IsDeleted, f => true), filter: folder => folder.HierarchicalPath!.Value.IsDescendantOf(drive.HierarchicalPath!.Value));
            await _unitOfWork.FileRepository.BatchUpdateAsync(file => file.SetProperty(f => f.IsDeleted, f => true), filter: file => file.HierarchicalPath!.Value.IsDescendantOf(drive.HierarchicalPath!.Value));
            await _unitOfWork.SaveAsync();

            _backgroundJobClient.Schedule(() => this.DeleteDrive(id), TimeSpan.FromDays(int.Parse(_configuration["schedule:deletedWaitDays"]!)));
        }

        public async Task<DriveDetailsResponseDto> Update(int id, DriveRequestDto request)
        {
            if (!await _permissionService.HasPermissionLoginUser(Role.Contributor, id, typeof(Drive)))
            {
                throw new ForbiddenException(ExceptionMessage.UnallowedModification);
            }

            var drive = await _unitOfWork.DriveRepository.GetByIdAsync(id);
            if (drive == null)
            {
                throw new NotFoundException(ExceptionMessage.DriveNotFound);
            }

            var user = await _userService.GetLoginUserAsync();
            if (drive.OwnerId != user.Id)
            {
                throw new ForbiddenException(ExceptionMessage.UnallowedModification);
            }

            drive = _mapper.Map(request, drive);
            _unitOfWork.DriveRepository.Update(drive);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<DriveDetailsResponseDto>(drive);
        }
    }
}
