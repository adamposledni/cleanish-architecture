using Onion.Core.Helpers;
using Onion.Core.Mapper;
using AM = AutoMapper;

namespace Onion.Infrastructure.Core.Mapper;

public class Mapper : IMapper
{
    private readonly AM.Mapper _mapperAdaptee;

    public Mapper()
    {
        AM.MapperConfiguration configuration = new(MappperProfile.Configure);
        _mapperAdaptee = new(configuration);
    }

    public TDest Map<TSource, TDest>(TSource source, Action<TDest> additionalProperties = null)
    {
        Guard.NotNull(source, nameof(source));

        return _mapperAdaptee.Map<TSource, TDest>(source, opts =>
        {
            opts.AfterMap((s, d) =>
            {
                additionalProperties?.Invoke(d);
            });
        });
    }

    public IEnumerable<TDest> MapCollection<TSource, TDest>(IEnumerable<TSource> sources, Action<TDest> additionalProperties = null)
    {
        Guard.NotNull(sources, nameof(sources));

        return sources.Select(s => Map(s, additionalProperties));
    }
}
