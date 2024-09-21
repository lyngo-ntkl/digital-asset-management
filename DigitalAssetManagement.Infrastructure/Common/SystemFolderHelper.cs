using Microsoft.Extensions.Hosting;

namespace DigitalAssetManagement.Infrastructure.Common
{
    public interface SystemFolderHelper
    {
        DirectoryInfo AddFolder(string folderName);
        DirectoryInfo AddFolder(string folderName, string parentAbsolutePath);
    }

    public class SystemFolderHelperImplementation: SystemFolderHelper
    {
        private const string BaseFolder = "Files";
        private readonly string BasePath;
        private const string FolderSeparator = @"\\";

        public SystemFolderHelperImplementation(IHostEnvironment env)
        {
            this.BasePath = $"{env.ContentRootPath}{FolderSeparator}{BaseFolder}";
        }

        public DirectoryInfo AddFolder(string folderName)
        {
            var path = $"{BasePath}{FolderSeparator}{folderName}";
            return Directory.CreateDirectory(path);
        }

        public DirectoryInfo AddFolder(string folderName, string parentAbsolutePath)
        {
            var path = $"{BasePath}{FolderSeparator}{parentAbsolutePath}{FolderSeparator}{folderName}";
            return Directory.CreateDirectory(path);
        }

        public void UploadFolder()
        {
            throw new NotImplementedException();
        }
    }
}
