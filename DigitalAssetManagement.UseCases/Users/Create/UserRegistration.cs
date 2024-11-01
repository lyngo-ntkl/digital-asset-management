namespace DigitalAssetManagement.UseCases.Users.Create
{
    public interface UserRegistration
    {
        Task Register(RegistrationRequest request);
    }
}
