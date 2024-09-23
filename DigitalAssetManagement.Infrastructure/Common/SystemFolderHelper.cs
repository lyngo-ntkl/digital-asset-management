using Microsoft.Extensions.Hosting;

namespace DigitalAssetManagement.Infrastructure.Common
{
    public interface SystemFolderHelper
    {
        DirectoryInfo AddFolder(string folderName, out string folderAbsolutePath);
        DirectoryInfo AddFolder(string folderName, string parentAbsolutePath, out string folderAbsolutePath);
    }

    public class SystemFolderHelperImplementation: SystemFolderHelper
    {
        private const string BaseFolder = "Files";
        private readonly string BasePath;
        private const string FolderSeparator = "\\";

        public SystemFolderHelperImplementation(IHostEnvironment env)
        {
            this.BasePath = $"{env.ContentRootPath}{FolderSeparator}{BaseFolder}{FolderSeparator}";
        }

        public DirectoryInfo AddFolder(string folderName, out string folderAbsolutePath)
        {
            var path = $"{BasePath}{folderName}";
            var directoryInfo = Directory.CreateDirectory(path);
            folderAbsolutePath = directoryInfo.FullName.Split(BasePath, StringSplitOptions.RemoveEmptyEntries)[0];
            return directoryInfo;
        }

        public DirectoryInfo AddFolder(string folderName, string parentAbsolutePath, out string folderAbsolutePath)
        {
            var path = $"{BasePath}{parentAbsolutePath}{FolderSeparator}{folderName}";
            var directoryInfo = Directory.CreateDirectory(path);
            folderAbsolutePath = directoryInfo.FullName.Split(BasePath, StringSplitOptions.RemoveEmptyEntries)[0];
            return directoryInfo;
        }

        public void UploadFolder()
        {
            throw new NotImplementedException();
        }
    }
}
