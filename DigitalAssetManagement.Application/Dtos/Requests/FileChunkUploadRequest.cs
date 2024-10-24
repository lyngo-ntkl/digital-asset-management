﻿namespace DigitalAssetManagement.Application.Dtos.Requests
{
    public class FileChunkUploadRequest
    {
        public required int FileId { get; set; }
        public required int TotalChunk { get; set; }
        public required int ChunkNumber { get; set; }
        public required byte[] ChunkData { get; set; }
    }
}
