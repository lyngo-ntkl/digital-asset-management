using Microsoft.Extensions.Hosting;

namespace DigitalAssetManagement.Infrastructure.Common
{
    public interface SystemFileHelper
    {
        string AddFile(Stream fileStream, string fileName, string parentPath);
        void DeleteFile(string absolutePath);
    }

    public class SystemFileHelperImplementation: SystemFileHelper
    {
        private const string BaseFolder = "Files";
        private readonly string BasePath;
        private readonly int FileBufferSize = 2048;
        private const string FolderSeparator = "\\";

        public SystemFileHelperImplementation(IHostEnvironment environment)
        {
            this.BasePath = $"{environment.ContentRootPath}{FolderSeparator}{BaseFolder}{FolderSeparator}";
        }

        public string AddFile(Stream fileStream, string fileName, string parentPath)
        {
            string fileAbsolutePath = $"{parentPath}{FolderSeparator}{fileName}";
            string fileRelativePath = $"{BasePath}{fileAbsolutePath}";
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

            return fileAbsolutePath;
        }

        public void DeleteFile(string absolutePath)
        {
            var path = $"{BasePath}{absolutePath}";
            File.Delete(path);
        }
    }
}
