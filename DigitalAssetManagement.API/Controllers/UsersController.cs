using DigitalAssetManagement.Application.Dtos.Requests.Users;
using DigitalAssetManagement.Application.Dtos.Responses.Folders;
using DigitalAssetManagement.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DigitalAssetManagement.API.Controllers
{
    [Route("/v1/api/users")]
    [ApiController]
    public class UsersController(UserService userService) : ControllerBase
    {
        private readonly UserService _userService = userService;

        [HttpPost("registration")]
        [AllowAnonymous]
        public async Task Register([FromBody] EmailPasswordRegistrationRequest request)
        {
            await _userService.Register(request);
        }
    }
}
