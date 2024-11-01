namespace DigitalAssetManagement.UseCases.Users.Login
{
    public interface LoginByEmailPassword
    {
        Task<AuthResponse> LoginByEmailAndPassword(EmailPasswordAuthRequest request);
    }
}
