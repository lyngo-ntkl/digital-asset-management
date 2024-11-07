using DigitalAssetManagement.UseCases.Users.Login;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace DigitalAssetManagement.API.Controllers
{
    [Route("/v1/api/auth")]
    [ApiController]
    public class AuthController(LoginByEmailPassword loginByEmailPassword) : ControllerBase
    {
        private readonly LoginByEmailPassword _loginByEmailPassword = loginByEmailPassword;

        [HttpPost("authentication")]
        [AllowAnonymous]
        public async Task<AuthResponse> LoginWithEmailPassword([FromBody] EmailPasswordAuthRequest request)
        {
            return await _loginByEmailPassword.LoginByEmailAndPassword(request);
        }
    }
}
