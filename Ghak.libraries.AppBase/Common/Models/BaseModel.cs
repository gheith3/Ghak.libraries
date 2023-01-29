using System.ComponentModel.DataAnnotations;

namespace Ghak.libraries.AppBase.Common.Models;

public class BaseModel
{
    [Key] 
    public string Id { get; set; } = Guid.NewGuid().ToString("N");

    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public DateTime? DeletedAt { get; set; }
}