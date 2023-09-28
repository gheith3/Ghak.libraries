using Microsoft.Extensions.Configuration;

namespace Ghak.libraries.AppBase.Utils;

public static class AppSettings
{
    private static IConfiguration? _configuration;

    public static void SetAppSetting(IConfiguration? configuration)
    {
        _configuration = configuration;
    }
    
    public static string GetFromAppSetting(string key)
    {
        if (_configuration == null)
        {
            throw new Exception($"SetAppSetting method not call when app start");
        }
        var val = _configuration.GetSection(key).Value;
        if (string.IsNullOrEmpty(val))
        {
            throw new Exception($"AppSetting {key} is not configured");
        }
        return val;
    }
}