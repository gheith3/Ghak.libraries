using Microsoft.Extensions.Configuration;

namespace Ghak.libraries.AppBase.Utils;

public static class AppSettingsEntrance
{
    private static IConfiguration? _configuration;

    public static void SetAppSetting(IConfiguration? configuration)
    {
        try
        {
            Console.WriteLine("set app configuration");
            _configuration = configuration;
        }
        catch (Exception e)
        {
            Console.WriteLine($"cant set app configuration {e.Message}");
            throw;
        }
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