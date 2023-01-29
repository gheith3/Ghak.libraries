namespace Ghak.libraries.AppBase.Common.Interfaces;

public interface ISoftDelete
{
    public DateTime? DeletedAt { get; set; }
}