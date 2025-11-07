using MottuChallenge.Application.DTOs.Response;

namespace MottuChallenge.Application.Pagination
{
    public sealed class PaginatedResult<T>
    {
        public IReadOnlyList<T> Items { get; }
        public int TotalItems { get; }
        public int Page { get; }
        public int PageSize { get; }
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
        public List<HateoasLink> Links { get; set; }

        public bool HasPrevious => Page > 1;

        public bool HasNext => Page < TotalPages;

        public PaginatedResult(IReadOnlyList<T> items, int totalItems, int page, int pageSize)
            => (Items, TotalItems, Page, PageSize) = (items, totalItems, page, pageSize);
    }
}
