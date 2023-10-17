using System.Reflection;
using Ghak.libraries.AppBase.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Ghak.libraries.AppBase.Extensions;

public static class SwaggerSettingsExtension
{
    
    public static IServiceCollection AppSwaggerDocSetting(this IServiceCollection services, bool isWithSecure = false)
    {
        var title = AppSettingsEntrance.GetFromAppSetting("SwaggerGen:Title");
        var version = AppSettingsEntrance.GetFromAppSetting("SwaggerGen:Version");
        var description = AppSettingsEntrance.GetFromAppSetting("SwaggerGen:Description");
        var contactName = AppSettingsEntrance.GetFromAppSetting("SwaggerGen:ContactName");
        var contactEmail = AppSettingsEntrance.GetFromAppSetting("SwaggerGen:ContactEmail");
        var licenseName = AppSettingsEntrance.GetFromAppSetting("SwaggerGen:LicenseName");
        var licenseUrl = AppSettingsEntrance.GetFromAppSetting("SwaggerGen:LicenseUrl");

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(gen =>
        {
            gen.EnableAnnotations();
            gen.SwaggerDoc(version, new OpenApiInfo
            {
                Title = title,
                Version = version,
                Description = description,
                Contact = new OpenApiContact
                {
                    Name = contactName,
                    Email = contactEmail,
                },
                License = new OpenApiLicense
                {
                    Name = licenseName,
                    Url = new Uri(licenseUrl)
                }
            });
            gen.UseAllOfToExtendReferenceSchemas();
            gen.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

            if (isWithSecure)
            {
                gen.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"Enter 'Bearer' [space] 'token'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                gen.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });
            }
            gen.SchemaFilter<XEnumNamesSchemaFilter>();
        });
        return services;
    }
    
    public static WebApplication UseAppSwaggerUI(this WebApplication app)
    {
        string title = AppSettingsEntrance.GetFromAppSetting("SwaggerGen:Title");
        string version = AppSettingsEntrance.GetFromAppSetting("SwaggerGen:Version");

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint($"/swagger/{version}/swagger.json", title);
        });
        return app;
    }
}