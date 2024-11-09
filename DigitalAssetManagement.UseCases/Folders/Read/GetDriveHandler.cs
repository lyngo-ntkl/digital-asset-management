using DigitalAssetManagement.UseCases.Common;
using DigitalAssetManagement.UseCases.Repositories;

namespace DigitalAssetManagement.UseCases.Folders.Read
{
    public class GetDriveHandler(IMetadataRepository metadataRepository, IJwtHelper jwtHelper, IMapper mapper) : IGetDrive
    {
        private readonly IMetadataRepository _metadataRepository = metadataRepository;
        private readonly IJwtHelper _jwtHelper = jwtHelper;
        private readonly IMapper _mapper = mapper;

        public async Task<FolderDetailResponse> GetDrive()
        {
            var loginUserId = int.Parse(_jwtHelper.ExtractSidFromAuthorizationHeader()!);
            var userDrive = await _metadataRepository.GetByUserIdAndTypeDrive(loginUserId);
            return _mapper.Map<FolderDetailResponse>(userDrive);
        }
    }
}
