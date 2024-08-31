using DigitalAssetManagement.Application.Dtos.Requests;
using DigitalAssetManagement.Application.Dtos.Responses;

namespace DigitalAssetManagement.Application.Services
{
    public interface FolderService
    {
        Task<FolderDetailResponseDto> Create(FolderCreationRequestDto request);
        Task<FolderDetailResponseDto> Get(int id);
    }
}
