using AutoMapper;
using Onion.Core.Helpers;
using Onion.Core.Mapper;

namespace Onion.Infrastructure.Core.Mapper;


// TODO: mapster
public class ObjectMapper : IObjectMapper
{
    private readonly IMapper _mapperAdaptee;

    public ObjectMapper(IMapper mapperAdaptee)
    {

        _mapperAdaptee = mapperAdaptee;
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
