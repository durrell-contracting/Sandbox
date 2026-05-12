using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using OpenXmlProcessor.Services;
using System.Xml;
using System.Xml.Linq;
using Text = DocumentFormat.OpenXml.Wordprocessing.Text;

namespace OpenXmlProcessor.OpenXml;

public class RunMerger
{
    private readonly IEncryptionService _encryptionService;
    private readonly int _renderWidth;

    private int _redactedElementsIndex = 0;
    private int _paragraphTextRunsIndex = 0;
    private Text _textRun = new Text();
    private RunProperties? _sourceProperties;
    private string _textRunText = string.Empty;
    private XElement _redactedElement = XElement.Parse("<empty/>");
    private string _redactedElementText = string.Empty;

    public RunMerger(IEncryptionService encryptionService, int renderWidth = 10)
    {
        _encryptionService = encryptionService ?? throw new ArgumentNullException(nameof(encryptionService));
        _renderWidth = renderWidth;
    }

    public Paragraph Merge(ParagraphProperties? pPr, List<XElement> redactedElements, List<Text> paragraphTextRuns)
    {
        var result = new Paragraph();
        if (pPr != null)
            result.AppendChild((ParagraphProperties)pPr.CloneNode(true));

        _redactedElementsIndex = 0;
        _paragraphTextRunsIndex = 0;
        _textRun = paragraphTextRuns[_paragraphTextRunsIndex];
        _sourceProperties = _textRun.Parent?.GetFirstChild<RunProperties>();
        _textRunText = _textRun.Text;
        _redactedElement = redactedElements[_redactedElementsIndex];
        _redactedElementText = _redactedElement.Value ?? string.Empty;
        while ((_redactedElementsIndex < redactedElements.Count) && (_paragraphTextRunsIndex < paragraphTextRuns.Count))
        {
            ProcessRedactedElement(redactedElements, paragraphTextRuns, result);
        }
        while (_paragraphTextRunsIndex < paragraphTextRuns.Count)
        {
            result.AppendChild(RunFactory.CreatePlaintextRun(_textRunText, _sourceProperties));
            MoveToNextTextRun(paragraphTextRuns);
        }

        return result;
    }

    private void ProcessRedactedElement(List<XElement> redactedElements, List<Text> paragraphTextRuns, Paragraph result)
    {
        int priorCharCount = 0;
        int trailingCharCount = 0;
        (priorCharCount, trailingCharCount) = EntityPositionInTextRun(_textRunText, _redactedElementText);
        if ((priorCharCount == 0) && (trailingCharCount == 0))
        {
            result.AppendChild(CreateRedactionCustomXmlRun(_redactedElement, _sourceProperties));
            MoveToNextTextRun(paragraphTextRuns);
            MoveToNextRedactedElement(redactedElements);
            return;
        }
        else if ((priorCharCount == 0) && (trailingCharCount < 0))
        {
            result.AppendChild(CreateRedactionCustomXmlRun(_redactedElement, _sourceProperties));
            while (trailingCharCount < 0)
            {
                MoveToNextTextRun(paragraphTextRuns);
                trailingCharCount += _textRunText.Length;
                if (trailingCharCount > 0)
                    _textRunText = _textRunText.Substring(_textRunText.Length - trailingCharCount);
                else if (trailingCharCount == 0)
                    MoveToNextTextRun(paragraphTextRuns);
            }
            MoveToNextRedactedElement(redactedElements);
            return;
        }
        else if ((priorCharCount == 0) && (trailingCharCount > 0))
        {
            result.AppendChild(CreateRedactionCustomXmlRun(_redactedElement, _sourceProperties));
            _textRunText = _textRunText.Substring(_textRunText.Length - trailingCharCount);
            MoveToNextRedactedElement(redactedElements);
            return;
        }
        if ((priorCharCount > 0) && (trailingCharCount == 0))
        {
            result.AppendChild(RunFactory.CreatePlaintextRun(_textRunText.Substring(0, priorCharCount), _sourceProperties));
            result.AppendChild(CreateRedactionCustomXmlRun(_redactedElement, _sourceProperties));
            MoveToNextTextRun(paragraphTextRuns);
            MoveToNextRedactedElement(redactedElements);
            return;
        }
        else if ((priorCharCount > 0) && (trailingCharCount < 0) && (priorCharCount < _textRunText.Length))
        {
            // Start of Entity Found
            result.AppendChild(RunFactory.CreatePlaintextRun(_textRunText.Substring(0, priorCharCount), _sourceProperties));
            result.AppendChild(CreateRedactionCustomXmlRun(_redactedElement, _sourceProperties));
            while (trailingCharCount < 0)
            {
                MoveToNextTextRun(paragraphTextRuns);
                trailingCharCount += _textRunText.Length;
                if (trailingCharCount > 0)
                    _textRunText = _textRunText.Substring(_textRunText.Length - trailingCharCount);
                else if (trailingCharCount == 0)
                    MoveToNextTextRun(paragraphTextRuns);
            }
            MoveToNextRedactedElement(redactedElements);
            return;
        }
        else if ((priorCharCount > 0) && (trailingCharCount < 0))
        {
            result.AppendChild(RunFactory.CreatePlaintextRun(_textRunText, _sourceProperties));
            MoveToNextTextRun(paragraphTextRuns);
            return;
        }
        else if ((priorCharCount > 0) && (trailingCharCount > 0))
        {
            result.AppendChild(RunFactory.CreatePlaintextRun(_textRunText.Substring(0, priorCharCount), _sourceProperties));
            result.AppendChild(CreateRedactionCustomXmlRun(_redactedElement, _sourceProperties));
            _textRunText = _textRunText.Substring(_textRunText.Length - trailingCharCount);
            MoveToNextRedactedElement(redactedElements);
            return;
        }
        else
            Console.Error.WriteLine($"Unexpected case in ProcessMatchingTextRun: priorCharCount={priorCharCount}, trailingCharCount={trailingCharCount}, textRun={_textRunText}, entity={_redactedElementText}");
    }

    private (int, int) EntityPositionInTextRun(string textRun, string entity)
    {
        int priorCharCount = 0;
        while ((priorCharCount < textRun.Length) && !textRun.Substring(priorCharCount).StartsWith(entity.Substring(0, Math.Min(entity.Length, textRun.Length - priorCharCount))))
            priorCharCount++;

        int trailingCharCount = textRun.Length - priorCharCount - entity.Length;

        return (priorCharCount, trailingCharCount);
    }

    private void MoveToNextTextRun(List<Text> paragraphTextRuns)
    {
        _paragraphTextRunsIndex++;
        if(_paragraphTextRunsIndex >= paragraphTextRuns.Count)
        {
            _textRun = new Text();
            _textRunText = string.Empty;
            _sourceProperties = null;
            return;
        }
        _textRun = paragraphTextRuns[_paragraphTextRunsIndex];
        _sourceProperties = _textRun.Parent?.GetFirstChild<RunProperties>();
        _textRunText = _textRun.Text;
    }

    private void MoveToNextRedactedElement(List<XElement> redactedElements)
    {
        _redactedElementsIndex++;
        if(_redactedElementsIndex >= redactedElements.Count)
        {
            _redactedElement = XElement.Parse("<Empty/>");
            _redactedElementText = string.Empty;
            return;
        }
        _redactedElement = redactedElements[_redactedElementsIndex];
        _redactedElementText = _redactedElement?.Value ?? string.Empty;
    }

    public CustomXmlRun CreateRedactionCustomXmlRun(XElement element, RunProperties? sourceProperties = null)
    {
        var encryptedText = _encryptionService.Encrypt(element.Value);
        var rendered = RenderedText(element);
        return RunFactory.CreateRedactionCustomXmlRun(encryptedText, rendered, sourceProperties);
    }

    private string RenderedText(XElement element)
    {
        string entity = element.Attribute("entity")?.Value ?? string.Empty;
        int paddingLength = Math.Max(2, (_renderWidth - entity.Length) / 2);
        return new string('X', paddingLength) + " " + entity + " " + new string('X', paddingLength);
    }
}