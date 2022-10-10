using MapsterMapper;
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

    public TDest Map<TDest>(object source, Action<TDest> additionalProperties = null)
    {
        Guard.NotNull(source, nameof(source));

        TDest dest = _mapperAdaptee.Map<TDest>(source);

        additionalProperties?.Invoke(dest);
        return dest;
    }

    public IEnumerable<TDest> MapCollection<TDest>(IEnumerable<object> sources, Action<TDest> additionalProperties = null)
    {
        Guard.NotNull(sources, nameof(sources));

        return sources.Select(s => Map(s, additionalProperties));
    }
}
