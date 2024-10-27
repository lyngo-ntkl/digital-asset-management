using AutoMapper;
using DigitalAssetManagement.Application.Common;
using DigitalAssetManagement.Application.Dtos.Requests.Users;
using DigitalAssetManagement.Application.Dtos.Responses.Users;
using DigitalAssetManagement.Application.Common.Exceptions;
using DigitalAssetManagement.Application.Repositories;
using DigitalAssetManagement.Application.Services;
using DigitalAssetManagement.Infrastructure.Common;
using DigitalAssetManagement.Infrastructure.PostgreSQL.DatabaseContext;

namespace DigitalAssetManagement.Infrastructure.Services
{
    public class UserServiceImplementation: UserService
    {
        private readonly UnitOfWork _unitOfWork;

        public UserServiceImplementation(UnitOfWork unitOfWork, 
            IMapper mapper, 
            DriveService driveService)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<User?> GetById(int id)
        {
            return await _unitOfWork.UserRepository.GetByIdAsync(id);
        }

        
    }
}
