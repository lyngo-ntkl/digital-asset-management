using DigitalAssetManagement.Application.Dtos.Responses.Folders;

namespace DigitalAssetManagement.Application.Dtos.Responses.Drives
{
    public class DriveDetailsResponseDto
    {
        public int Id { get; set; }
        public string? DriveName { get; set; }
        public ICollection<FolderResponseDto>? Folders { get; set; }
        public ICollection<FileResponseDto>? Files { get; set; }
        public bool IsDeleted { get; set; }
    }
}
