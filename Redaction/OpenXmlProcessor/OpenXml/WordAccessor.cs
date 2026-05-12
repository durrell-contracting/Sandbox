using System;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using OpenXmlProcessor.Services;

namespace OpenXmlProcessor.OpenXml;

public class WordAccessor : IWordAccessor
{
    private readonly WordprocessingDocument _document;
    private readonly ParagraphRedactor _paragraphRedactor;
    private readonly ParagraphDecryptor _paragraphDecryptor;
    private int _paragraphCount = 0;
    private int _runCount = 0;

    public WordAccessor(string filename, IRedactionService redactionService, IEncryptionService encryptionService)
    {
        if (filename is null) throw new ArgumentNullException(nameof(filename));
        _document = WordprocessingDocument.Open(filename, true, new OpenSettings { AutoSave = false });
        _paragraphRedactor = new ParagraphRedactor(redactionService, encryptionService);
        _paragraphDecryptor = new ParagraphDecryptor(encryptionService);
    }

    public void Write()
    {
        _document.Save();
    }

    public static void RedactWord(string file)
    {
        using IWordAccessor accessor = new WordAccessor(file, new RedactionService(new FakedEntityRecognitionService()), new EncryptionService());
        accessor.RedactAsync().Wait();
        accessor.Write();
    }

    public async Task RedactAsync()
    {
        var body = _document.MainDocumentPart?.Document?.Body;
        if (body == null)
        {
            Console.WriteLine("No document body found.");
            return;
        }

        Console.WriteLine();
        Console.WriteLine("----- Original Word Document -----");
        Read();

        Console.WriteLine();
        Console.WriteLine("----- Processing ... -----");
        foreach (var paragraph in body.Descendants<Paragraph>().ToList())
        {
            var redactedParagraph = await _paragraphRedactor.RedactAsync(paragraph);
            body.ReplaceChild(redactedParagraph, paragraph);
        }

        Console.WriteLine();
        Console.WriteLine("----- Redacted Word Document -----");
        Read();

        Console.WriteLine();
        Console.WriteLine("----- Processing ... (Decrypt) -----");
        foreach (var paragraph in body.Descendants<Paragraph>().ToList())
        {
            _paragraphDecryptor.Decrypt(paragraph);
        }

        Console.WriteLine();
        Console.WriteLine("----- Unredacted Word Document -----");
        Read();

        // Basic text extraction: prints raw text in document body.
        var bodyText = body.InnerText;
        Console.WriteLine();
        Console.WriteLine("----- Word Document Text -----");
        Console.WriteLine(string.IsNullOrWhiteSpace(bodyText) ? "<no text found>" : bodyText);
        Console.WriteLine("------------------------------");
    }

    public void Read()
    {
        Body body = _document.MainDocumentPart!.Document!.Body!;
        foreach (var paragraph in body.Descendants<Paragraph>())
        {
            ReadParagraph(paragraph);
        }
    }

    private void ReadParagraph(Paragraph paragraph)
    {
        _paragraphCount++;
        _runCount = 0;
        foreach (var run in paragraph.Descendants<Run>().ToList())
        {
            ReadRun(run);
        }
    }

    private void ReadRun(Run run)
    {
        _runCount++;
        var text = string.Concat(run.Descendants<DocumentFormat.OpenXml.Wordprocessing.Text>().Select(t => t.Text));
        Console.WriteLine($"{_paragraphCount}\t{_runCount}:\t'{text}'");
    }

    public void Dispose()
    {
        _document.Dispose();
    }
}

public interface IWordAccessor : IDisposable
{
    Task RedactAsync();
    void Read();
    void Write();
}