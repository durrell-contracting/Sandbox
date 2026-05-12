namespace OpenXmlProcessor.Services;

public interface IEntityRecognitionService
{
    Task<string> RecognizeEntitiesAsync(string text);
    string Decrypt(string encryptedText);
}
