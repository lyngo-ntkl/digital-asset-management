using DigitalAssetManagement.UseCases.Common;
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

        public void AddFolder(string absolutePath)
        {
            var path = $"{BasePath}{absolutePath}";
            var directoryInfo = Directory.CreateDirectory(path);
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
