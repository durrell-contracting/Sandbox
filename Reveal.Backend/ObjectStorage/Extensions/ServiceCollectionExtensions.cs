using Microsoft.Extensions.DependencyInjection;
using Minio;
using Reveal.ObjectStorage.Configuration;
using Reveal.ObjectStorage.Services;

namespace Reveal.ObjectStorage.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddObjectStorage(
        this IServiceCollection services,
        ObjectStorageConfig? config = null)
    {
        config ??= new ObjectStorageConfig();

        services.AddScoped(_ => config);

        IMinioClient minioClient = new MinioClient()
            .WithEndpoint(config.Endpoint)
            .WithCredentials(config.AccessKey, config.SecretKey)
            .Build();
        services.AddSingleton<IMinioClient>(minioClient);

        services.AddScoped<IObjectStorageService, ObjectStorageService>();

        return services;
    }
}
