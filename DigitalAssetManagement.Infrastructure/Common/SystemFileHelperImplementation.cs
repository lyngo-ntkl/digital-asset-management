using Microsoft.Extensions.Hosting;

namespace DigitalAssetManagement.Infrastructure.Common
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

    public class SystemFileHelperImplementation: SystemFileHelper
    {
        private readonly string BasePath;
        private readonly int FileBufferSize = 5 * 1024 * 1024;

        public SystemFileHelperImplementation(IHostEnvironment environment)
        {
            this.BasePath = $"{environment.ContentRootPath}";
        }

        public string AddFile(byte[] bytes, string absolutePath)
        {
            string fileRelativePath = $"{BasePath}{absolutePath}";
            var writtenStream = new FileStream(fileRelativePath, FileMode.CreateNew, FileAccess.Write);
            var streamWriter = new BinaryWriter(writtenStream);

            streamWriter.Write(bytes);

            streamWriter.Close();
            writtenStream.Close();

            return absolutePath;
        }

        public string AddFile(Stream fileStream, string absolutePath)
        {
            string fileRelativePath = $"{BasePath}{absolutePath}";
            var writtenStream = new FileStream(fileRelativePath, FileMode.CreateNew, FileAccess.Write);
            var streamReader = new BinaryReader(fileStream);
            var streamWriter = new BinaryWriter(writtenStream);

            byte[] buffer = new byte[FileBufferSize];
            int readBytes = streamReader.Read(buffer, 0, FileBufferSize);
            while (readBytes > 0)
            {
                streamWriter.Write(buffer, 0, readBytes);
                readBytes = streamReader.Read(buffer, 0, FileBufferSize);
            }

            streamWriter.Close();
            streamReader.Close();
            writtenStream.Close();
            fileStream.Close();

            return absolutePath;
        }

        public void DeleteFile(string absolutePath)
        {
            var path = $"{BasePath}{absolutePath}";
            File.Delete(path);
        }

        public async Task<byte[]> GetFile(string absolutePath)
        {
            var path = $"{BasePath}{absolutePath}";
            return await File.ReadAllBytesAsync(path);
        }

        public void MergeFile(string destinationAbsolutePath, List<string> fileChunkAbsolutePaths)
        {
            byte[] buffer = new byte[FileBufferSize];
            int readByteCount;

            var destPath = $"{BasePath}{destinationAbsolutePath}";
            var writtenStream = new FileStream(destPath, FileMode.CreateNew, FileAccess.Write);
            var writer = new BinaryWriter(writtenStream);

            foreach (string srcAbsolutePath in fileChunkAbsolutePaths)
            {
                var srcPath = $"{BasePath}{srcAbsolutePath}";
                var chunkStream = new FileStream(srcPath, FileMode.Open, FileAccess.Read);
                var reader = new BinaryReader(chunkStream);

                readByteCount = reader.Read(buffer, 0, buffer.Length);
                while (readByteCount > 0)
                {
                    writer.Write(buffer, 0, readByteCount);
                    readByteCount = reader.Read(buffer, 0, buffer.Length);
                }

                chunkStream.Close();
                reader.Close();

                DeleteFile(srcAbsolutePath);
            }

            writtenStream.Close();
            writer.Close();
        }

        public string MoveFile(string oldAbsolutePath, string newParentAbsolutePath)
        {
            var newFileAbsolutePath = AbsolutePathCreationHelper.GetNewAbsolutePath(oldAbsolutePath, newParentAbsolutePath);
            var srcPath = $"{BasePath}{oldAbsolutePath}";
            var destPath = $"{BasePath}{newFileAbsolutePath}";
            File.Move(srcPath, destPath);
            return newFileAbsolutePath;
        }
    }
}
