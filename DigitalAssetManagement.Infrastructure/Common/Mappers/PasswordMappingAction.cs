using AutoMapper;
using DigitalAssetManagement.Application.Dtos.Requests.Users;
using DigitalAssetManagement.Domain.Entities;
using DigitalAssetManagement.Infrastructure.PostgreSQL.DatabaseContext;

namespace DigitalAssetManagement.Infrastructure.Common.Mappers
{
    public class PasswordMappingAction : IMappingAction<EmailPasswordRegistrationRequest, User>
    {
        private readonly HashingHelper _hashingHelper;

        public PasswordMappingAction(HashingHelper hashingHelper)
        {
            _hashingHelper = hashingHelper;
        }

        public void Process(EmailPasswordRegistrationRequest source, User destination, ResolutionContext context)
        {
            _hashingHelper.Hash(source.Password, out string salt, out string hash);
            destination.PasswordHash = hash;
            destination.PasswordSalt = salt;
        }
    }
}
