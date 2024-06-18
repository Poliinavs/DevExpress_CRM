using AutoMapper;
using DevExpress.ExpressApp;

namespace CRM.Utils
{
    public class MappingConfiguration<TSource, TDestination>
    {
        private IMapper mapper;

        public MappingConfiguration()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TSource, TDestination>();
            });

            mapper = config.CreateMapper();
        }

        public TDestination Map(TSource source, TDestination destination, IObjectSpace objectSpace)
        {
            return mapper.Map<TSource, TDestination>(source, destination, opts => opts.ConstructServicesUsing(type => objectSpace));
        }
    }
}
