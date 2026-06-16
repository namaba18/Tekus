using Tekus.Domain.Entities.Common;

namespace Tekus.Domain.Entities
{
    public class Service : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public decimal HourlyRate { get; set; }

        public Guid SupplierId { get; set; }
        public Supplier Supplier { get; set; } = null!;
    }
}
