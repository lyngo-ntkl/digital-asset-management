
using DigitalAssetManagement.UseCases.Common;
using DigitalAssetManagement.UseCases.Repositories;

namespace DigitalAssetManagement.UseCases.Users.Read
{
    public class GetUsersHandler(UserRepository userRepository, IMapper mapper): GetUsers
    {
        private readonly UserRepository _userRepository = userRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<ICollection<UserResponse>> GetUsersAsync(string email, int pageSize, int page)
        {
            var users = await _userRepository.GetByContainingEmailWithPaginationAsync(email, pageSize, page);
            return _mapper.Map<ICollection<UserResponse>>(users);
        }
    }
}
