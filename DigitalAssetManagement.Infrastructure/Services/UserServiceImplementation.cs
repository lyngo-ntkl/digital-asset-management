using AutoMapper;
using DigitalAssetManagement.Application.Common;
using DigitalAssetManagement.Application.Dtos.Requests.Users;
using DigitalAssetManagement.Application.Dtos.Responses.Users;
using DigitalAssetManagement.Application.Exceptions;
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

        public UserServiceImplementation(UnitOfWork unitOfWork, IMapper mapper, HashingHelper hashingHelper, JwtHelper jwtHelper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _hashingHelper = hashingHelper;
            _jwtHelper = jwtHelper;
        }

        public async Task<AuthResponse> LoginWithEmailPassword(EmailPasswordAuthRequest request)
        {
            var user = await _unitOfWork.UserRepository.GetByEmailAsync(request.Email);
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
            if (_unitOfWork.UserRepository.ExistByEmail(request.Email))
            {
                throw new BadRequestException(ExceptionMessage.RegisteredEmail);
            }

            var user = _mapper.Map<User>(request);

            await _unitOfWork.UserRepository.InsertAsync(user);
            await _unitOfWork.SaveAsync();
        }
    }
}
