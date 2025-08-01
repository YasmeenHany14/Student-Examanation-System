﻿namespace WebApi.Helpers;

using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;

public static class JsonPatchInputFormatter
{
    public static NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter()
    {
        var builder = new ServiceCollection()
            .AddLogging()
            .AddMvc()
            // .AddNewtonsoftJson(setupAction =>
            // {
            //     setupAction.SerializerSettings.ContractResolver =
            //         new CamelCasePropertyNamesContractResolver();
            // })
            .AddNewtonsoftJson()
            .Services.BuildServiceProvider();

        return builder
            .GetRequiredService<IOptions<MvcOptions>>()
            .Value
            .InputFormatters
            .OfType<NewtonsoftJsonPatchInputFormatter>()
            .First();
    }
}