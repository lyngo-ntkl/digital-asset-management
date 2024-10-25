using DigitalAssetManagement.Application.Dtos.Requests.Users;
using DigitalAssetManagement.Application.Dtos.Responses.Users;
using DigitalAssetManagement.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitalAssetManagement.API.Controllers
{
    [Route("/v1/api/auth")]
    [ApiController]
    public class AuthController(UserService userService) : ControllerBase
    {
        private readonly UserService _userService = userService;

        [HttpPost("authentication")]
        [AllowAnonymous]
        public async Task<AuthResponse> LoginWithEmailPassword([FromBody] EmailPasswordAuthRequest request)
        {
            return await _userService.LoginByEmailAndPassword(request);
        }
    }
}
