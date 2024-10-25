using AutoMapper;
using DigitalAssetManagement.Application.Common;
using DigitalAssetManagement.Application.Dtos.Requests.Users;
using DigitalAssetManagement.Application.Dtos.Responses.Users;
using DigitalAssetManagement.Application.Common.Exceptions;
using DigitalAssetManagement.Application.Repositories;
using DigitalAssetManagement.Application.Services;
using DigitalAssetManagement.Domain.Entities;
using DigitalAssetManagement.Infrastructure.Common;

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

        public async Task<AuthResponse> LoginByEmailAndPassword(EmailPasswordAuthRequest request)
        {
            var user = await _unitOfWork.UserRepository.GetByEmailAsync(request.Email);
            if (user == null)
            {
                throw new BadRequestException(ExceptionMessage.UnregisteredEmail);
            }

            var passwordHash = _hashingHelper.Hash(request.Password, user.PasswordSalt);
            if (passwordHash != user.PasswordHash)
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
            if (await _unitOfWork.UserRepository.ExistByEmailAsync(request.Email))
            {
                throw new BadRequestException(ExceptionMessage.RegisteredEmail);
            }

            var user = await Add(_mapper.Map<User>(request));

            await _driveService.AddNewDrive(user.Id, user.Name);
        }

        public async Task<User?> GetById(int id)
        {
            return await _unitOfWork.UserRepository.GetByIdAsync(id);
        }

        public async Task<User> GetByEmail(string email)
        {
            var user = await _unitOfWork.UserRepository.GetByEmailAsync(email)
                ?? throw new NotFoundException(ExceptionMessage.UserNotFound);
            return user;
        }
    }
}
