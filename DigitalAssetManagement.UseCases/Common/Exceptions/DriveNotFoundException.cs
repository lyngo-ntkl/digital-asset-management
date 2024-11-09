namespace DigitalAssetManagement.UseCases.Common.Exceptions
{
    public class DriveNotFoundException : NotFoundException
    {
        public DriveNotFoundException(): base("Drive not found") { }
    }
}
