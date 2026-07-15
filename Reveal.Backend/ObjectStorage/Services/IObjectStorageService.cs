namespace Reveal.ObjectStorage.Services;

public interface IObjectStorageService
{
    Task<string> UploadFileAsync(string filename, Stream fileStream, string contentType);
    Task<Stream> DownloadFileAsync(string fileKey);
    Task DeleteFileAsync(string fileKey);
}
