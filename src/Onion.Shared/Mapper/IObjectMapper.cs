namespace Onion.Shared.Mapper;

public interface IObjectMapper
{
    TDest Map<TDest>(object source, Action<TDest> additionalProperties);
    TDest Map<TDest>(object source);
    IEnumerable<TDest> MapCollection<TDest>(IEnumerable<object> sources, Action<TDest> additionalProperties = null);
}
