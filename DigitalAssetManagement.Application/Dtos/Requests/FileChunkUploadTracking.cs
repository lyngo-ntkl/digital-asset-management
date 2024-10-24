namespace DigitalAssetManagement.Application.Dtos.Requests
{
    public class FileChunkUploadTracking
    {
        public int TotalChunk { get; set; }
        public Dictionary<int, string> ArrivedChunks { get; set; }
    }
}
