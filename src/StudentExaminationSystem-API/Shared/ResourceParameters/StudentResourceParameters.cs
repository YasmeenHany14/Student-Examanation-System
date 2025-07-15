using System.Text.Json.Serialization;

namespace Shared.ResourceParameters;

public class StudentResourceParameters : BaseResourceParameters
{
    public string? SearchQuery { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
    // define order attributes, of key value pairs, where key is attribute name in database, value is exposed name in API
    public Dictionary<string, string> OrderAttributes { get; set; } = new()
    {
        { "firstName", "First Name" },
        { "lastName", "Last Name" },
        { "email", "Email" },
        { "studentId", "Student ID" },
        { "age", "BirthDate"}
    };
}
