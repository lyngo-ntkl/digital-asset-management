namespace DigitalAssetManagement.UseCases.Common
{
    public interface SystemFileHelper
    {
        string AddFile(byte[] bytes, string absolutePath);
        string AddFile(Stream fileStream, string absolutePath);
        void DeleteFile(string absolutePath);
        Task<byte[]> GetFile(string absolutePath);
        void MergeFile(string destinationAbsolutePath, List<string> fileChunkAbsolutePath);
        string MoveFile(string oldAbsolutePath, string newParentAbsolutePath);
    }
}
