using DigitalAssetManagement.Application.Dtos.Requests.Folders;
using DigitalAssetManagement.Application.Dtos.Responses.Folders;

namespace DigitalAssetManagement.Application.Services
{
    public interface FolderService
    {
        Task<FolderDetailResponseDto> AddNewFolder(FolderCreationRequestDto request);
        //Task<FolderDetailResponseDto> Update(int id, FolderModificationRequestDto request);
        //Task<FolderDetailResponseDto> Get(int id);
        //Task<FolderDetailResponseDto> MoveFolder(int id, FolderMovementRequestDto request);
        //Task MoveToTrash(int id);
        //Task Delete(int id);
        Task DeleteFolder(int id);
    }
}
