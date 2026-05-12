using System;
using System.Linq;
using DocumentFormat.OpenXml.Wordprocessing;
using OpenXmlProcessor.Services;

namespace OpenXmlProcessor.OpenXml;

public class ParagraphDecryptor
{
    private readonly IEncryptionService _encryptionService;

    public ParagraphDecryptor(IEncryptionService encryptionService)
    {
        _encryptionService = encryptionService ?? throw new ArgumentNullException(nameof(encryptionService));
    }

    /// <summary>
    /// Replaces redacted CustomXmlRun elements with plaintext runs containing decrypted text.
    /// </summary>
    public void Decrypt(Paragraph paragraph)
    {
        if (paragraph == null) throw new ArgumentNullException(nameof(paragraph));

        foreach (var redactedRun in paragraph.Descendants<CustomXmlRun>().ToList())
        {
            var encryptedRun = redactedRun.Elements<Run>().FirstOrDefault();
            var properties = encryptedRun?.GetFirstChild<RunProperties>();
            string encryptedText = encryptedRun?.InnerText ?? string.Empty;
            string decryptedText = string.Empty;

            if (!string.IsNullOrEmpty(encryptedText))
                decryptedText = _encryptionService.Decrypt(encryptedText);

            properties?.RemoveAllChildren<Hidden>(); // Remove encryption styling from decrypted run
            var plaintext = RunFactory.CreatePlaintextRun(decryptedText, properties);
            paragraph.InsertBefore(plaintext, redactedRun);
            paragraph.RemoveChild(redactedRun);
        }
    }
}