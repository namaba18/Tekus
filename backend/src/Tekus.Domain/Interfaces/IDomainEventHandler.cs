namespace Tekus.Domain.Interfaces
{
    public interface IDomainEventHandler<TEvent>
    {
        Task Handle(TEvent domainEvent);
    }
}
