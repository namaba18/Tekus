using Tekus.Domain.Interfaces;

namespace IntegrationTests.Common
{
    /// <summary>Test double that records every dispatched domain event for later assertions.</summary>
    public class RecordingEventDispatcher : IEventDispatcher
    {
        public List<object> DispatchedEvents { get; } = [];

        public Task DispatchAsync(object domainEvent)
        {
            DispatchedEvents.Add(domainEvent);
            return Task.CompletedTask;
        }
    }
}
