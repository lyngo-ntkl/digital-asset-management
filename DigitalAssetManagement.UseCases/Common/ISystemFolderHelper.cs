namespace DigitalAssetManagement.UseCases.Common
{
    public interface ISystemFolderHelper
    {
        void AddFolder(string absolutePath);
        void DeleteFolder(string absolutePath);
        void MoveFolder(string oldAbsolutePath, string newAbsolutePath);
    }
}
