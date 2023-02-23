namespace Offers.Api.Helpers;

public class PaginationWrapperBase
{
    public int Page { get; set; }

    public int MaxPageSize => 100;

    private int _pageSize;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value <= MaxPageSize ? value : MaxPageSize;
    }

    public long TotalItems { get; set; }
    public int TotalPages => PageSize != 0 ? (int) Math.Ceiling((decimal) TotalItems / PageSize) : 0;

    public bool HasPrevious => Page > 1;
    public bool HasNext => Page < TotalPages;
}

public class PaginationWrapper<T> : PaginationWrapperBase
    where T : class
{
    private IEnumerable<T>? _data;

    public IEnumerable<T> Data => _data ??= new List<T>();

    public PaginationWrapper(IEnumerable<T> data)
    {
        _data = data;
    }
}