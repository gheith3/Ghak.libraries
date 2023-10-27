namespace Ghak.libraries.AppBase.Models;

public class ListItem<T>
{
    public T Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public Dictionary<string, object> Data { get; set; } = new();
}