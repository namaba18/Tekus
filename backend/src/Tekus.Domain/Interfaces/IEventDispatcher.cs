namespace Tekus.Domain.Interfaces
{
    public interface IEventDispatcher
    {
        Task DispatchAsync(object domainEvent);
    }
}
