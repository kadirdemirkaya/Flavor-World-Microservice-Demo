namespace BasketService.Application.Abstractions
{
    public interface IPubEventService
    {
        Task PublishDomainEventAsync(string serviceName);
    }
}
