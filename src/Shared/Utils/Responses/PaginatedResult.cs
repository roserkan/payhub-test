using Shared.Utils.Pagination;

namespace Shared.Utils.Responses;

public class PaginatedResult<T> : BasePageableModel
{
    public IList<T> Items
    {
        get => _items ??= new List<T>();
        set => _items = value;
    }

    private IList<T>? _items;
}