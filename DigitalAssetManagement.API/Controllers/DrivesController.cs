using DigitalAssetManagement.Application.Dtos.Responses.Drives;
using DigitalAssetManagement.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace DigitalAssetManagement.API.Controllers
{
    [Route("v1/api/drives")]
    [ApiController]
    public class DrivesController : ControllerBase
    {
        private readonly DriveService _driveService;

        public DrivesController(DriveService driveService)
        {
            _driveService = driveService;
        }

        [HttpGet]
        public async Task<List<DriveResponseDto>> GetDriveOwnedByLoginUser([FromQuery] string? name)
        {
            return await _driveService.GetDriveOwnedByLoginUser(name);
        }

        [HttpGet("{id}")]
        public async Task<DriveDetailsResponseDto> GetById([FromRoute] int id)
        {
            return await _driveService.GetById(id);
        }
    }
}
