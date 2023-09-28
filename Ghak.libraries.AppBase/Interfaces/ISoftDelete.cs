namespace Ghak.libraries.AppBase.Interfaces;

public interface ISoftDelete
{
    public DateTime? DeletedAt { get; set; }
}