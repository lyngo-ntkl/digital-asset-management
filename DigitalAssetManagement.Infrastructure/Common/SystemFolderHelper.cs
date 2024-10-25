using Microsoft.Extensions.Hosting;

namespace DigitalAssetManagement.Infrastructure.Common
{
    

    public class SystemFolderHelperImplementation: SystemFolderHelper
    {
        private const string BaseFolder = "Files";
        private readonly string BasePath;
        private const string FolderSeparator = "/";

        public SystemFolderHelperImplementation(IHostEnvironment env)
        {
            this.BasePath = $"{env.ContentRootPath}{FolderSeparator}{BaseFolder}{FolderSeparator}";
        }

        public DirectoryInfo AddFolder(string folderName, out string folderAbsolutePath)
        {
            folderAbsolutePath = folderName;
            var path = $"{BasePath}{folderAbsolutePath}";
            var directoryInfo = Directory.CreateDirectory(path);
            return directoryInfo;
        }

        public DirectoryInfo AddFolder(string folderName, string parentAbsolutePath, out string folderAbsolutePath)
        {
            folderAbsolutePath = $"{parentAbsolutePath}{FolderSeparator}{folderName}";
            var path = $"{BasePath}{folderAbsolutePath}";
            var directoryInfo = Directory.CreateDirectory(path);
            return directoryInfo;
        }

        public void DeleteFolder(string absolutePath)
        {
            var path = $"{BasePath}{absolutePath}";
            Directory.Delete(path, recursive: true);
        }

        public string MoveFolder(string oldFolderAbsolutePath, string newParentAbsolutePath)
        {
            var srcRelativePath = $"{BasePath}{oldFolderAbsolutePath}";
            var folderName = oldFolderAbsolutePath.Split(FolderSeparator).Last();
            var newFolderAbsolutePath = $"{newParentAbsolutePath}{FolderSeparator}{folderName}";
            var destRelativePath = $"{BasePath}{newFolderAbsolutePath}";
            Directory.Move(srcRelativePath, destRelativePath);
            return newFolderAbsolutePath;
        }
    }
}
