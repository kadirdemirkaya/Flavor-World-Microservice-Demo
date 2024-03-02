namespace AuthenticationService.Application.Abstractions
{
    public interface IPubEventService
    {
        Task PublishDomainEventAsync(string serviceName);
    }
}
