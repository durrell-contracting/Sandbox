using DocumentFormat.OpenXml.Presentation;
using DocumentFormat.OpenXml.Wordprocessing;
using OpenXmlProcessor.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Text = DocumentFormat.OpenXml.Wordprocessing.Text;

namespace OpenXmlProcessor.OpenXml;

public class ParagraphRedactor
{
    private readonly IRedactionService _redactionService;
    private readonly IEncryptionService _encryptionService;
    private readonly int _renderWidth = 10;

    public ParagraphRedactor(IRedactionService redactionService, IEncryptionService encryptionService)
    {
        _redactionService = redactionService ?? throw new ArgumentNullException(nameof(redactionService));
        _encryptionService = encryptionService ?? throw new ArgumentNullException(nameof(encryptionService));
    }

    /// <summary>
    /// Returns a new Paragraph containing plaintext runs and redacted CustomXmlRun runs.
    /// Preserves paragraph properties.
    /// </summary>
    public async Task<Paragraph> RedactAsync(Paragraph paragraph)
    {
        if (paragraph == null) throw new ArgumentNullException(nameof(paragraph));

        // Extract text for redaction
        string plaintext = string.Concat(paragraph.Descendants<Text>().Select(t => t.Text));
        if (string.IsNullOrEmpty(plaintext))
            return new Paragraph();

        string redactedText = await _redactionService.RedactAsync(plaintext);

        var redactedXml = XElement.Parse("<root>" + redactedText + "</root>"); // Validate redaction output is well-formed XML
        var redactedElements = redactedXml.Descendants().ToList(); // Force immediate parsing to catch errors early
        var paragraphTextRuns = paragraph.Descendants<Text>().ToList();

        try
        {
            Paragraph result = MergeRuns(paragraph.GetFirstChild<ParagraphProperties>(), redactedElements, paragraphTextRuns);

            return result;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
            throw;
        }
    }

    private Paragraph MergeRuns(ParagraphProperties? pPr, List<XElement> redactedElements, List<Text> paragraphTextRuns)
    {
        RunMerger merger = new(_encryptionService, _renderWidth);
        return merger.Merge(pPr, redactedElements, paragraphTextRuns);
    }
}