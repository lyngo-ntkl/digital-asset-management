namespace DigitalAssetManagement.Application.Dtos.Responses
{
    public class FileResponseDto
    {
        public required string FileName { get; set; }
        public required byte[] FileContent { get; set; }
    }
}
