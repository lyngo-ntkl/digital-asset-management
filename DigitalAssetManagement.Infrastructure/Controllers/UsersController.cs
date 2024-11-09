using DigitalAssetManagement.UseCases.Users.Create;
using DigitalAssetManagement.UseCases.Users.Read;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitalAssetManagement.Infrastructure.Controllers
{
    [Route("/v1/api/users")]
    [ApiController]
    public class UsersController(UserRegistration userRegistration, GetUsers getUsers) : ControllerBase
    {
        private readonly UserRegistration _userRegistration = userRegistration;
        private readonly GetUsers _getUsers = getUsers;

        [HttpPost()]
        [AllowAnonymous]
        public async Task Register([FromBody] RegistrationRequest request)
        {
            await _userRegistration.RegisterAsync(request);
        }

        [HttpGet]
        public async Task<ICollection<UserResponse>> GetUsers([FromQuery] string email, [FromQuery] int pageSize = 10, [FromQuery] int page = 1)
        {
            return await _getUsers.GetUsersAsync(email, pageSize, page);
        }
    }
}
