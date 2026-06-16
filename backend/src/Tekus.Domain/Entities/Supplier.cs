using Tekus.Domain.Entities.Common;

namespace Tekus.Domain.Entities
{
    public class Supplier : BaseEntity
    {
        public string NIT { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string WebPage { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
