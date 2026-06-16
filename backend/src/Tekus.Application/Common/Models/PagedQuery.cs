namespace Tekus.Application.Common.Models
{
    /// <summary>Common pagination, sorting and search parameters shared by list queries.</summary>
    public record PagedQuery
    {
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
        public string? SearchTerm { get; init; }
        public string? SortBy { get; init; }
        public bool SortDescending { get; init; }
    }
}
