namespace DigitalAssetManagement.UseCases.Common
{
    public interface SystemFolderHelper
    {
        void AddFolder(string absolutePath);
        void DeleteFolder(string absolutePath);
        string MoveFolder(string oldFolderAbsolutePath, string newParentAbsolutePath);
    }
}
