namespace WebApi.Helpers;

using System.Text.Json;

public static class JsonHelper
{
    public static string SerializeWithCustomOptions(object data)
    {
        var options = new JsonSerializerOptions
        {
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };
        return JsonSerializer.Serialize(data, options);
    }
}
