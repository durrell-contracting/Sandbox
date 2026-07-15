using Microsoft.Extensions.DependencyInjection;
using Reveal.MessageBroker.Configuration;
using Reveal.MessageBroker.Services;

namespace Reveal.MessageBroker.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMessageBroker(
        this IServiceCollection services,
        MessageBrokerConfig? config = null)
    {
        config ??= new MessageBrokerConfig();

        // TODO:  Configure MassTransit and RabbitMQ here

        services.AddScoped(_ => config);

        services.AddScoped<IMessageBrokerService, MessageBrokerService>();

        return services;
    }
}
