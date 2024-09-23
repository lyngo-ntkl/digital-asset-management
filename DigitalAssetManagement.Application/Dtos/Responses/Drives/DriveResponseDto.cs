namespace DigitalAssetManagement.Application.Dtos.Responses.Drives
{
    public class DriveResponseDto
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public bool IsDeleted { get; set; }
    }
}
