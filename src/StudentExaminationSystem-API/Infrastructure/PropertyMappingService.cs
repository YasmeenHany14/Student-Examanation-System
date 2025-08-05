using Domain.DTOs;
using Domain.Models;

namespace Infrastructure;

public interface IPropertyMappingService
{
    Dictionary<string, PropertyMappingValue>
        GetPropertyMapping<TSource, TDestination>();
}

public class PropertyMappingService : IPropertyMappingService
{
    private readonly Dictionary<string, PropertyMappingValue> _studentPropertyMapping =
        new(StringComparer.OrdinalIgnoreCase)
        {
            { "Name", new PropertyMappingValue(["User.FirstName", "User.LastName"]) },
            { "Age", new PropertyMappingValue(["User.DateOfBirth"], true) },
            { "EnrollmentDate", new PropertyMappingValue(["EnrollmentDate"]) }
        };
    private readonly IList<IPropertyMapping> _propertyMappings = new List<IPropertyMapping>();
    
    public PropertyMappingService()
    {
        _propertyMappings.Add(new PropertyMapping<GetStudentByIdInfraDto, Student>(_studentPropertyMapping));
    }

    public Dictionary<string, PropertyMappingValue>
        GetPropertyMapping<TSource, TDestination>()
    {
        var matchingMapping = _propertyMappings
            .OfType<PropertyMapping<TSource, TDestination>>().ToList();
        if (matchingMapping.Count == 0)
            throw new Exception();
        return matchingMapping.FirstOrDefault()!.MappingDictionary;
    }
}

public class PropertyMapping<TSource, TDestination> : IPropertyMapping
{
    public Dictionary<string, PropertyMappingValue> MappingDictionary { get; }

    public PropertyMapping(Dictionary<string, PropertyMappingValue> mappingDictionary)
    {
        MappingDictionary = mappingDictionary ?? throw new ArgumentNullException(nameof(mappingDictionary));
    }
}

public class PropertyMappingValue
{
    public IEnumerable<string> DestinationProperties { get; }
    public bool Revert { get; }

    public PropertyMappingValue(
        IEnumerable<string> destinationProperties,
        bool revert = false)
    {
        DestinationProperties = destinationProperties ?? throw new ArgumentNullException(nameof(destinationProperties));
        Revert = revert;
    }
}

public interface IPropertyMapping
{
    // marker interface for property mappings 
}

// register this as a transient in the DI container