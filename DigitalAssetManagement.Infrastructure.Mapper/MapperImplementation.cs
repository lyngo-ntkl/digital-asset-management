using AutoMapper;

namespace DigitalAssetManagement.Infrastructure.Mapper
{
    public class MapperImplementation(IMapper mapper) : UseCases.Common.IMapper
    {
        private readonly IMapper _mapper = mapper;

        public TDestination Map<TDestination>(object source)
        {
            return _mapper.Map<TDestination>(source);
        }

        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            return _mapper.Map(source, destination);
        }
    }
}
