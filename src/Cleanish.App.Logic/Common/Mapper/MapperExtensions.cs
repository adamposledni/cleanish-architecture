using Mapster;

namespace Cleanish.App.Logic.Common.Mapper;

public static class MapperExtensions
{
    public static TDestination Adapt<TDestination>(this object source, Action<TDestination> additionalProperties = null)
    {
        var dest = source.Adapt<TDestination>(TypeAdapterConfig.GlobalSettings);
        additionalProperties?.Invoke(dest);
        return dest;
    }
}