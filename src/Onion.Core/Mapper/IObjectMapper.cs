namespace Onion.Core.Mapper;

public interface IObjectMapper
{
    TDest Map<TDest>(object source, Action<TDest> additionalProperties = null);
    IEnumerable<TDest> MapCollection<TDest>(IEnumerable<object> sources, Action<TDest> additionalProperties = null);
}
