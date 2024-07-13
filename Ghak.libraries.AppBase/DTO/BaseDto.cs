namespace Ghak.libraries.AppBase.DTO;

public class BaseDto
{
    public string Id { get; set; } = Ulid.NewUlid().ToString();
    public bool IsActive { get; set; }
    public string ActivationStatus => IsActive ? "Active" : "Disabled";
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; } 
}