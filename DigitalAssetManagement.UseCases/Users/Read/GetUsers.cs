namespace DigitalAssetManagement.UseCases.Users.Read
{
    public interface GetUsers
    {
        Task<ICollection<UserResponse>> GetUsersAsync(string email, int pageSize, int page);
    }
}
