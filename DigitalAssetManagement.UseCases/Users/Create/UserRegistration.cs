namespace DigitalAssetManagement.UseCases.Users.Create
{
    public interface UserRegistration
    {
        Task RegisterAsync(RegistrationRequest request);
    }
}
