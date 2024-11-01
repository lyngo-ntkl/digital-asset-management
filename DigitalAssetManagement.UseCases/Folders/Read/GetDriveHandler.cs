using DigitalAssetManagement.UseCases.Common;
using DigitalAssetManagement.UseCases.Repositories;

namespace DigitalAssetManagement.UseCases.Folders.Read
{
    public class GetDriveHandler(MetadataRepository metadataRepository, JwtHelper jwtHelper, Mapper mapper) : GetDrive
    {
        private readonly MetadataRepository _metadataRepository = metadataRepository;
        private readonly JwtHelper _jwtHelper = jwtHelper;
        private readonly Mapper _mapper = mapper;

        public async Task<FolderDetailResponse> GetDrive()
        {
            var loginUserId = int.Parse(_jwtHelper.ExtractSidFromAuthorizationHeader()!);
            var userDrive = await _metadataRepository.GetByUserIdAndTypeDrive(loginUserId);
            return _mapper.Map<FolderDetailResponse>(userDrive);
        }
    }
}
