using System.ComponentModel.DataAnnotations;

namespace Ghak.libraries.AppBase.Utils;

public class PaginationListArgs : ListArgs
{
    [Required] public int Page { get; set; } = 1;
    [Required] public int ItemsPeerPage { get; set; } = 50;
    
    public string? OrderBy { get; set; } = "Id";
    public string? OrderDirection { get; set; } = "DESC";

    public int GetPagNumber()
    {
        return Page > 0 ? Page : 1;
    }
    
    public int GetItemsPeerPage()
    {
        return ItemsPeerPage > 0 ? ItemsPeerPage : 20;
    }
}