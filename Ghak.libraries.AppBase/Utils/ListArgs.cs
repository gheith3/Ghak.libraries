namespace Ghak.libraries.AppBase.Utils;

public class ListArgs
{
    public string? SearchQuery { get; set; } = null;
    public Dictionary<string, object> Args { get; set; } = new();
}