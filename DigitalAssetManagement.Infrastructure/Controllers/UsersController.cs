using DigitalAssetManagement.UseCases.Users.Create;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitalAssetManagement.API.Controllers
{
    [Route("/v1/api/users")]
    [ApiController]
    public class UsersController(UserRegistration userRegistration) : ControllerBase
    {
        private readonly UserRegistration _userRegistration= userRegistration;

        [HttpPost("registration")]
        [AllowAnonymous]
        public async Task Register([FromBody] RegistrationRequest request)
        {
            await _userRegistration.Register(request);
        }
    }
}
