using System.Text.Json.Serialization;

namespace Shared.ResourceParameters;

public class StudentResourceParameters : BaseResourceParameters
{
    public string? SearchQuery { get; set; }
    public string? Name { get; set; }
}
