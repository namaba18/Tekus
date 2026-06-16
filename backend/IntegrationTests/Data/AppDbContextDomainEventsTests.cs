using IntegrationTests.Common;
using Tekus.Domain.Entities;
using Tekus.Domain.Events;

namespace IntegrationTests.Data
{
    public class AppDbContextDomainEventsTests : SqliteIntegrationTestBase
    {
        private RecordingEventDispatcher Dispatcher => (RecordingEventDispatcher)EventDispatcher;

        public AppDbContextDomainEventsTests() : base(new RecordingEventDispatcher())
        {
        }

        [Fact]
        public async Task SaveChangesAsync_DispatchesServiceCreatedEvent_WhenServiceIsAdded()
        {
            var supplier = new Supplier { NIT = "1", Name = "Acme", WebPage = "", Email = "a@a.com" };
            Context.Suppliers.Add(supplier);
            await Context.SaveChangesAsync();

            var service = Service.Create("Soporte", 99m, supplier.Id);
            Context.Services.Add(service);
            await Context.SaveChangesAsync();

            var dispatched = Assert.Single(Dispatcher.DispatchedEvents);
            var serviceCreatedEvent = Assert.IsType<ServiceCreatedEvent>(dispatched);
            Assert.Equal(service.Id, serviceCreatedEvent.ServiceId);
            Assert.Equal("Soporte", serviceCreatedEvent.ServiceName);
            Assert.Equal(supplier.Id, serviceCreatedEvent.SupplierId);
        }

        [Fact]
        public async Task SaveChangesAsync_ClearsDomainEvents_AfterDispatching()
        {
            var supplier = new Supplier { NIT = "1", Name = "Acme", WebPage = "", Email = "a@a.com" };
            Context.Suppliers.Add(supplier);
            await Context.SaveChangesAsync();

            var service = Service.Create("Soporte", 99m, supplier.Id);
            Context.Services.Add(service);
            await Context.SaveChangesAsync();

            Assert.Empty(service.DomainEvents);
        }
    }
}
