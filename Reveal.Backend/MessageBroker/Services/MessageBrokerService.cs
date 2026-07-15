using MassTransit;

namespace Reveal.MessageBroker.Services;

public class MessageBrokerService : IMessageBrokerService
{
    private readonly IPublishEndpoint _publishEndpoint;

    public MessageBrokerService(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
    }

    public Task PublishMpegUploadedEventAsync(string fileId, string fileName, string storageLocation)
    {
        // Implementation for publishing the event
        return Task.CompletedTask;
    }
}
