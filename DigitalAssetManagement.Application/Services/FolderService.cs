using DigitalAssetManagement.Application.Dtos.Requests;
using DigitalAssetManagement.Application.Dtos.Requests.Folders;
using DigitalAssetManagement.Application.Dtos.Responses.Folders;

namespace DigitalAssetManagement.Application.Services
{
    public interface FolderService
    {
        
        //Task<FolderDetailResponseDto> Update(int id, FolderModificationRequestDto request);
        Task DeleteFolderSoftly(int id);
    }
}
