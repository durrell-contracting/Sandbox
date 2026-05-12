using DocumentFormat.OpenXml.Wordprocessing;

namespace OpenXmlProcessor.Services;

public class RedactionService : IRedactionService
{
        private readonly IEntityRecognitionService _entityRecognitionService;
    
        public RedactionService(IEntityRecognitionService entityRecognitionService)
        {
            _entityRecognitionService = entityRecognitionService;
        }
    
        /// <inherit/>
        public async Task<string> RedactAsync(string text)
        {
            return await _entityRecognitionService.RecognizeEntitiesAsync(text);
    }

    public Task<Paragraph> RedactAsync(Paragraph paragraph)
    {
        throw new NotImplementedException();
    }
}

public interface IRedactionService
{
    /// <summary>
    /// Redacts sensitive information from the given text.
    /// </summary>
    /// <param name="text">The text to redact.</param>
    /// <returns>The redacted text.</returns>
    Task<string> RedactAsync(string text);
    Task<Paragraph> RedactAsync(Paragraph paragraph);
}
