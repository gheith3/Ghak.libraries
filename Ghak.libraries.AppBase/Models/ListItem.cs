namespace Ghak.libraries.AppBase.Models;

public class ListItem
{
    public string Id { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public Dictionary<string, object> Data { get; set; } = new();
}