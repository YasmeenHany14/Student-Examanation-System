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
            if (destProp != null && destProp.CanWrite && destProp.PropertyType == prop.PropertyType)
            {
                destProp.SetValue(destination, prop.GetValue(source));
            }
        }
        return destination;
    }
}
