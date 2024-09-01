using DigitalAssetManagement.Application.Dtos.Requests.Users;
using DigitalAssetManagement.Application.Dtos.Responses.Users;
using DigitalAssetManagement.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace DigitalAssetManagement.API.Controllers
{
    [Route("/v1/api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserService _userService;

        public AuthController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("authentication")]
        public async Task<AuthResponse> LoginWithEmailPassword([FromBody] EmailPasswordAuthRequest request)
        {
            return await _userService.LoginWithEmailPassword(request);
        }
    }
}
