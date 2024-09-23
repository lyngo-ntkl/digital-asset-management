using AutoMapper;
using DigitalAssetManagement.Application.Common;
using DigitalAssetManagement.Application.Dtos.Requests.Users;
using DigitalAssetManagement.Application.Dtos.Responses.Users;
using DigitalAssetManagement.Application.Exceptions;
using DigitalAssetManagement.Application.Repositories;
using DigitalAssetManagement.Application.Services;
using DigitalAssetManagement.Domain.Entities;
using DigitalAssetManagement.Infrastructure.Common;
using Microsoft.AspNetCore.Http;

namespace DigitalAssetManagement.Infrastructure.Services
{
    public class UserServiceImplementation: UserService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly HashingHelper _hashingHelper;
        private readonly JwtHelper _jwtHelper;
        private readonly DriveService _driveService;

        public UserServiceImplementation(UnitOfWork unitOfWork, 
            IMapper mapper, 
            HashingHelper hashingHelper, 
            JwtHelper jwtHelper, 
            DriveService driveService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _hashingHelper = hashingHelper;
            _jwtHelper = jwtHelper;
            _driveService = driveService;
        }

        private async Task<User> Add(User user)
        {
            user = await _unitOfWork.UserRepository.AddAsync(user);
            await _unitOfWork.SaveAsync();
            return user;
        }

        public async Task<AuthResponse> LoginWithEmailPassword(EmailPasswordAuthRequest request)
        {
            var user = await _unitOfWork.UserRepository.GetFirstOnConditionAsync(u => u.Email == request.Email);
            if (user == null)
            {
                throw new BadRequestException(ExceptionMessage.UnregisteredEmail);
            }

            _hashingHelper.Hash(request.Password, user.PasswordSalt, out string hash);
            if (hash != user.PasswordHash)
            {
                throw new BadRequestException(ExceptionMessage.UnmatchedPassword);
            }

            return new AuthResponse
            {
                AccessToken = _jwtHelper.GenerateAccessToken(user)
            };
        }

        public async Task Register(EmailPasswordRegistrationRequest request)
        {
            if (_unitOfWork.UserRepository.ExistByCondition(u => u.Email == request.Email))
            {
                throw new BadRequestException(ExceptionMessage.RegisteredEmail);
            }

            var user = await Add(_mapper.Map<User>(request));
            await _driveService.AddNewDrive(user.Id!.Value);
        }

        public async Task<User?> Get(int id)
        {
            return await _unitOfWork.UserRepository.GetByIdAsync(id);
        }
    }
}
