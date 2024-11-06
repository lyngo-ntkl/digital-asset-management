namespace DigitalAssetManagement.UseCases.Files.Create
{
    public class FileChunkUploadTracking
    {
        public int FileId { get; set; }
        public int TotalChunk { get; set; }
        public Dictionary<int, string> ArrivedChunks { get; set; } = null!;
    }
}
