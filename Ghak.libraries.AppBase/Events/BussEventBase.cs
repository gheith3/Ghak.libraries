namespace Ghak.libraries.AppBase.Events;

public class BussEventBase<T>
{
    public string Event { get; set; }
    public T Data { get; set; }
}