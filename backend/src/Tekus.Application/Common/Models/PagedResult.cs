namespace Tekus.Application.Common.Models
{
    public record PagedResult<T>(List<T> Items, int TotalCount, int PageNumber, int PageSize)
    {
        public int TotalPages => PageSize <= 0 ? 0 : (int)Math.Ceiling(TotalCount / (double)PageSize);
    }
}
