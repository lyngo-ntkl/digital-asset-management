namespace DigitalAssetManagement.UseCases.Common
{
    public interface Mapper
    {
        TDestination Map<TDestination>(object source);
        TDestination Map<TSource, TDestination>(TSource source, TDestination destination);
    }
}
