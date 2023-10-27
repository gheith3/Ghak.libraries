namespace Ghak.libraries.AppBase.DTO;

public class BaseToModifyDto<T>
{
    public T? Data { get; set; }
    public Dictionary<string, string> Comments { get; set; }
}