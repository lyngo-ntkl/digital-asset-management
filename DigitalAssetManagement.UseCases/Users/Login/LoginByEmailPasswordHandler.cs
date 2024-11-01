using DigitalAssetManagement.UseCases.Common.Exceptions;
using DigitalAssetManagement.UseCases.Repositories;
using DigitalAssetManagement.UseCases.Common;

namespace DigitalAssetManagement.UseCases.Users.Login
{
    public class LoginByEmailPasswordHandler(UserRepository userRepository, HashingHelper hashingHelper, JwtHelper jwtHelper): LoginByEmailPassword
    {
        private readonly UserRepository _userRepository = userRepository;
        private readonly HashingHelper _hashingHelper = hashingHelper;
        private readonly JwtHelper _jwtHelper = jwtHelper;

        public async Task<AuthResponse> LoginByEmailAndPassword(EmailPasswordAuthRequest request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null)
            {
                throw new BadRequestException(ExceptionMessage.UnregisteredEmail);
            }

            var passwordHash = _hashingHelper.Hash(request.Password, user.PasswordSalt);
            if (passwordHash != user.PasswordHash)
            {
                throw new BadRequestException(ExceptionMessage.UnmatchedPassword);
            }

            return new AuthResponse
            {
                AccessToken = _jwtHelper.GenerateAccessToken(user)
            };
        }
    }
}
