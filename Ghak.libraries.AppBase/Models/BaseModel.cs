using System.ComponentModel.DataAnnotations;
using Ghak.libraries.AppBase.Utils;

namespace Ghak.libraries.AppBase.Models;

public class BaseModel
{
    [Key] 
    public string Id { get; set; } = Helpers.GetStringKey();

    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public DateTime? DeletedAt { get; set; } = null;
}