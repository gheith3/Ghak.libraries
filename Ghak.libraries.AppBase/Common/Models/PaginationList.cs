namespace Ghak.libraries.AppBase.Common.Models;

public class PaginationList<T>
{
    private const int MaxPageSize = 250;
    private int _pageSize;


    public PaginationList()
    {
        Items = new List<T>();
    }

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
    }

    public int CurrentPage { get; set; }
    public int CurrentPageItems => Items.Count;
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }

    public int? NextPage
    {
        get
        {
            if (CurrentPage >= TotalPages) return null;

            return CurrentPage + 1;
        }
    }

    public int? PrevPage
    {
        get
        {
            if (CurrentPage <= 1)
                return null;
            if (CurrentPage > TotalPages) return TotalPages;

            return CurrentPage - 1;
        }
    }

    public int FirstItemOrder
    {
        get
        {
            if (TotalItems <= 0)
            {
                return 0;
            }

            return (PageSize * (CurrentPage - 1)) + 1;
        }
    }

    public IList<T> Items { get; set; }
}