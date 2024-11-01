namespace DigitalAssetManagement.UseCases.Files.Read
{
    public class FileContentResponse
    {
        public required string FileName { get; set; }
        public required byte[] FileContent { get; set; }
    }
}
