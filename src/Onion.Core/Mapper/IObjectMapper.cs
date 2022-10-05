namespace Onion.Core.Mapper;

public interface IObjectMapper
{
    TDest Map<TSource, TDest>(TSource source, Action<TDest> additionalProperties = null);
    IEnumerable<TDest> MapCollection<TSource, TDest>(IEnumerable<TSource> sources, Action<TDest> additionalProperties = null);
}
