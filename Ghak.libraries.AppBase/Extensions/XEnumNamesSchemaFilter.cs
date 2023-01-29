using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Ghak.libraries.AppBase.Extensions;

public class XEnumNamesSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        var type = context.Type;
        if (type.IsEnum)
        {
            // Add enum type information once
            if (schema.Extensions.ContainsKey("x-enumNames")) return;

            var valuesArr = new OpenApiArray();
            valuesArr.AddRange(Enum.GetNames(context.Type)
                .Select(value => new OpenApiString(value)));

            schema.Extensions.Add(
                "x-enumNames",
                valuesArr
            );
        }
    }
}