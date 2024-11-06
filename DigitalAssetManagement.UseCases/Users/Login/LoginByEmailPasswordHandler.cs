using DigitalAssetManagement.UseCases.Common.Exceptions;
using DigitalAssetManagement.UseCases.Repositories;
using DigitalAssetManagement.UseCases.Common;
using DigitalAssetManagement.Entities.DomainEntities;

namespace DigitalAssetManagement.UseCases.Users.Login
{
    public class LoginByEmailPasswordHandler(UserRepository userRepository, IHashingHelper hashingHelper, IJwtHelper jwtHelper): LoginByEmailPassword
    {
        private readonly UserRepository _userRepository = userRepository;
        private readonly IHashingHelper _hashingHelper = hashingHelper;
        private readonly IJwtHelper _jwtHelper = jwtHelper;

        public async Task<AuthResponse> LoginByEmailAndPassword(EmailPasswordAuthRequest request)
        {
            var user = await CheckUserExistance(request.Email);
            IsPasswordMatch(request.Password, user.PasswordHash, user.PasswordSalt);
            return new AuthResponse
            {
                AccessToken = _jwtHelper.GenerateAccessToken(user)
            };
        }

        private async Task<User> CheckUserExistance(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                throw new BadRequestException(ExceptionMessage.UnregisteredEmail);
            }
            return user;
        }

        private void IsPasswordMatch(string inputPassword, string passwordHash, string passwordSalt)
        {
            var inputPasswordHash = _hashingHelper.Hash(inputPassword, passwordSalt);
            if (inputPasswordHash != passwordHash)
            {
                throw new BadRequestException(ExceptionMessage.UnmatchedPassword);
            }
        }
    }
}
