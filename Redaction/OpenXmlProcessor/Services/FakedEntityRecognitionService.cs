namespace OpenXmlProcessor.Services;

public class FakedEntityRecognitionService : IEntityRecognitionService
{
    /// <inherit/>
    public async Task<string> RecognizeEntitiesAsync(string text)
    {
        // Simulate entity recognition logic
        await Task.Delay(100); // Simulate async work
        return text.Replace("Jack Sparrow", "<e entity=\"Name\">Jack Sparrow</e>");
    }

    /// <inherit/>
    public string Decrypt(string encryptedText)
    {
        return "Jack Sparrow";
    }
}
