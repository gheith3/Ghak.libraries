using System.ComponentModel.DataAnnotations;

namespace Ghak.libraries.AppBase.Utils;

public class PaginationListArgs
{
    [Required] public int Page { get; set; } = 1;
    [Required] public int ItemsPeerPage { get; set; } = 25;
    public string? SearchQuery { get; set; } = null;
    
    public string? OrderBy { get; set; } = "Id";
    public string? OrderDirection { get; set; } = "DESC";

    public Dictionary<string, object> Args { get; set; } = new();

    public int GetPagNumber()
    {
        return Page > 0 ? Page : 1;
    }
    
    public int GetItemsPeerPage()
    {
        return ItemsPeerPage > 0 ? ItemsPeerPage : 20;
    }
}