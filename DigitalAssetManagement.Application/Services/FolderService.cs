using DigitalAssetManagement.Application.Dtos.Requests.Folders;
using DigitalAssetManagement.Application.Dtos.Responses.Folders;

namespace DigitalAssetManagement.Application.Services
{
    public interface FolderService
    {
        Task<FolderDetailResponseDto> Create(FolderCreationRequestDto request);
        Task<FolderDetailResponseDto> Get(int id);
        Task<FolderDetailResponseDto> MoveFolder(int id, FolderMovementRequestDto request);
    }
}
