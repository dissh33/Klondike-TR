namespace Offers.Infrastructure.Helpers;

internal class PaginationWrapperBase
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

internal class PaginationWrapper<T> : PaginationWrapperBase
    where T : class, new()
{
    private IEnumerable<T>? _data;

    public IEnumerable<T>? Data
    {
        get => _data ??= new List<T>();
        set => _data = value;
    }

    public PaginationWrapper(IEnumerable<T> data)
    {
        _data = data;
    }
}

internal class PaginationWrapper<T, TViewModel> : PaginationWrapperBase
    where T : class, new()
{
    private IEnumerable<TViewModel>? _data;

    public IEnumerable<TViewModel>? Data
    {
        get => _data ??= new List<TViewModel>();
        set => _data = value;
    }

    public PaginationWrapper(PaginationWrapper<T> wrapper, Func<T, TViewModel> converting)
    {
        Page = wrapper.Page;
        PageSize = wrapper.PageSize;
        TotalItems = wrapper.TotalItems;

        _data = wrapper.Data?.Select(converting);
    }
}
