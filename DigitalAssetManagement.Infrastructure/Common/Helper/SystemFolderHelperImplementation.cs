using DigitalAssetManagement.UseCases.Common;

namespace DigitalAssetManagement.Infrastructure.Common.Helper
{
    public class SystemFolderHelperImplementation(IHostEnvironment env) : ISystemFolderHelper
    {
        private const string BaseFolder = "Files";
        private readonly string BasePath = $"{env.ContentRootPath}{FolderSeparator}{BaseFolder}{FolderSeparator}";
        private const string FolderSeparator = "/";

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

        public void MoveFolder(string oldAbsolutePath, string newAbsolutePath)
        {
            var srcRelativePath = $"{BasePath}{oldAbsolutePath}";
            var destRelativePath = $"{BasePath}{newAbsolutePath}";
            Directory.Move(srcRelativePath, destRelativePath);
        }
    }
}
