using Minio;

namespace Reveal.ObjectStorage.Services;

public class ObjectStorageService : IObjectStorageService
{
    private readonly IMinioClient _minioClient;
    private readonly string _bucketName;

    public ObjectStorageService(IMinioClient minioClient, string bucketName = "mpeg-files")
    {
        _minioClient = minioClient ?? throw new ArgumentNullException(nameof(minioClient));
        _bucketName = bucketName;
    }

    public async Task<string> UploadFileAsync(string filename, Stream fileStream, string contentType)
    {
        try
        {
            // Use the correct PutObjectAsync overload with PutObjectArgs
            var putObjectArgs = new Minio.DataModel.Args.PutObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(filename)
                .WithStreamData(fileStream)
                .WithObjectSize(fileStream.Length)
                .WithContentType(contentType);

            await _minioClient.PutObjectAsync(putObjectArgs);
            return $"{_bucketName}/{filename}";
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to upload file to MinIO: {ex.Message}", ex);
        }
    }

    public async Task<Stream> DownloadFileAsync(string fileKey)
    {
        try
        {
            var memoryStream = new MemoryStream();
            var parts = fileKey.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            var filename = parts.Length > 1 ? parts[1] : fileKey;

            var getObjectArgs = new Minio.DataModel.Args.GetObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(filename)
                .WithCallbackStream(stream => stream.CopyTo(memoryStream));

            await _minioClient.GetObjectAsync(getObjectArgs);

            memoryStream.Position = 0;
            return memoryStream;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to download file from MinIO: {ex.Message}", ex);
        }
    }

    private static readonly char[] separator = ['/'];
    public async Task DeleteFileAsync(string fileKey)
    {
        try
        {
            var parts = fileKey.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            var filename = parts.Length > 1 ? parts[1] : fileKey;

            var removeObjectArgs = new Minio.DataModel.Args.RemoveObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(filename);

            await _minioClient.RemoveObjectAsync(removeObjectArgs);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to delete file from MinIO: {ex.Message}", ex);
        }
    }
}
