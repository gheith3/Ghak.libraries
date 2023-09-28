using System.ComponentModel.DataAnnotations;
using Ghak.libraries.AppBase.Interfaces;
using Ghak.libraries.AppBase.Utils;

namespace Ghak.libraries.AppBase.Models;

public class BaseModel : ISoftDelete
{
    [Key] 
    public string Id { get; set; } = Helpers.GetStringKey();

    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DeletedAt { get; set; }
    
    public bool DeleteSoftly () => DeletedAt != DateTime.UtcNow;
}