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

        public UserServiceImplementation(UnitOfWork unitOfWork, IMapper mapper, HashingHelper hashingHelper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _hashingHelper = hashingHelper;
        }

        public Task<AuthResponse> LoginWithEmailPassword(EmailPasswordAuthRequest request)
        {
            throw new NotImplementedException();
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
