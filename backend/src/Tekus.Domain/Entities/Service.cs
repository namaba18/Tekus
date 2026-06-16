using Tekus.Domain.Entities.Common;
using Tekus.Domain.Events;

namespace Tekus.Domain.Entities
{
    public class Service : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public decimal HourlyRate { get; set; }

        public Guid SupplierId { get; set; }
        public Supplier Supplier { get; set; } = null!;

        private Service() { } // EF Core

        public static Service Create(string name, decimal hourlyRate, Guid supplierId)
        {
            var service = new Service
            {
                Name = name,
                HourlyRate = hourlyRate,
                SupplierId = supplierId
            };

            service.AddDomainEvent(new ServiceCreatedEvent(service.Id, service.Name, service.HourlyRate, service.SupplierId));

            return service;
        }
    }
}
