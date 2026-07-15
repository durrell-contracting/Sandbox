namespace Reveal.MessageBroker.Services;

public interface IMessageBrokerService
{
    Task PublishMpegUploadedEventAsync(string fileId, string fileName, string storageLocation);
}
