using DigitalAssetManagement.Application.Dtos.Requests;
using DigitalAssetManagement.Application.Dtos.Requests.Folders;
using DigitalAssetManagement.Application.Dtos.Responses.Folders;

namespace DigitalAssetManagement.Application.Services
{
    public interface FolderService
    {
        Task<FolderDetailResponseDto> AddNewFolder(FolderCreationRequestDto request);
        Task AddFolderPermission(int folderId, PermissionRequestDto request);
        //Task<FolderDetailResponseDto> Update(int id, FolderModificationRequestDto request);
        //Task<FolderDetailResponseDto> Get(int id);
        //Task MoveToTrash(int id);
        Task DeleteFolder(int id);
        Task MoveFolder(int folderId, int newParentId);
    }
}
