namespace DigitalAssetManagement.UseCases.Files.Create
{
    public class FileChunkUploadRequest
    {
        public required int FileId { get; set; }
        public required int TotalChunk { get; set; }
        public required int ChunkNumber { get; set; }
        public required byte[] Data { get; set; }
    }
}
