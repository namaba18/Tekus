using Tekus.Domain.Interfaces;

namespace IntegrationTests.Common
{
    /// <summary>Test double that swallows domain events; used when a test doesn't care about dispatching.</summary>
    public class NoOpEventDispatcher : IEventDispatcher
    {
        public Task DispatchAsync(object domainEvent) => Task.CompletedTask;
    }
}
