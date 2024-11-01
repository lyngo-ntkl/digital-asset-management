namespace DigitalAssetManagement.UseCases.Users.Login
{
    public class AuthResponse
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
