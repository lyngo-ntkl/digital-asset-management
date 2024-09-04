using AutoMapper;
using DigitalAssetManagement.Application.Common;
using DigitalAssetManagement.Application.Dtos.Responses.Drives;
using DigitalAssetManagement.Application.Exceptions;
using DigitalAssetManagement.Application.Repositories;
using DigitalAssetManagement.Application.Services;
using DigitalAssetManagement.Domain.Entities;
using System.Linq.Expressions;

namespace DigitalAssetManagement.Infrastructure.Services
{
    public class DriveServiceImplementation : DriveService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserService _userService;

        public DriveServiceImplementation(UnitOfWork unitOfWork, IMapper mapper, UserService userService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<DriveDetailsResponseDto> GetById(int id)
        {
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
    }
}
