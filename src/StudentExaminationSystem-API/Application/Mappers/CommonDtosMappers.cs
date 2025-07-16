namespace Application.Mappers;

public static class CommonDtosMappers
{
    public static TDestination MapTo<TSource, TDestination>(this TSource source)
        where TDestination : class, new()
    {
        if (source == null)
        {
            return null;
        }

        var destination = new TDestination();
        foreach (var prop in typeof(TSource).GetProperties())
        {
            var destProp = typeof(TDestination).GetProperty(prop.Name);
            if (destProp != null && destProp.CanWrite)
            {
                var sourceType = prop.PropertyType;
                var destType = destProp.PropertyType;
                var sourceUnderlying = Nullable.GetUnderlyingType(sourceType) ?? sourceType;
                var destUnderlying = Nullable.GetUnderlyingType(destType) ?? destType;

                if (sourceUnderlying == destUnderlying)
                {
                    destProp.SetValue(destination, prop.GetValue(source));
                }
            }
        }
        return destination;
    }
}
