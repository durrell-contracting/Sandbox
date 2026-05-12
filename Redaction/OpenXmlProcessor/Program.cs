// Copilot Prompt:  Read an OpenXml Document File passed as a command line parameter.

using System;
using System.IO;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Presentation;
using Text = DocumentFormat.OpenXml.Wordprocessing.Text;
using Run = DocumentFormat.OpenXml.Wordprocessing.Run;
using OpenXmlProcessor.OpenXml;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

if (ArgsIsEmpty())
{
    PrintUsage();
    return;
}

var path = Environment.GetCommandLineArgs().Skip(1).First();
if (!File.Exists(path))
{
    Console.Error.WriteLine($"File not found: {path}");
    return;
}

var ext = Path.GetExtension(path).ToLowerInvariant();
try
{
    switch (ext)
    {
        case ".docx":
        case ".docm":
            WordAccessor.RedactWord(path);
            break;
        case ".xlsx":
        case ".xlsm":
            ReadExcel(path);
            break;
        case ".pptx":
        case ".pptm":
            ReadPowerPoint(path);
            break;
        default:
            Console.Error.WriteLine($"Unsupported or unknown OpenXML extension: {ext}");
            break;
    }
}
catch (Exception ex)
{
    Console.Error.WriteLine($"Error reading file: {ex.Message}");
}

static bool ArgsIsEmpty()
{
    // Environment.GetCommandLineArgs()[0] is the exe; we require at least one argument after that.
    return Environment.GetCommandLineArgs().Length < 2;
}

static void PrintUsage()
{
    Console.WriteLine("Usage: OpenXmlProcessor <path-to-openxml-file>");
    Console.WriteLine("Supported: .docx/.docm (Word), .xlsx/.xlsm (Excel), .pptx/.pptm (PowerPoint)");
}

static void ReadWord(string file)
{
    using var doc = WordprocessingDocument.Open(file, false);
    var body = doc.MainDocumentPart?.Document?.Body;
    if (body == null)
    {
        Console.WriteLine("No document body found.");
        return;
    }

    int paragraphCount = 0;
    foreach(var child in body.Descendants<Paragraph>())
    {
        paragraphCount++;
        int runCount = 0;
        foreach (var run in child.Descendants<Run>())
        {
            runCount++;
            var text = string.Concat(run.Descendants<Text>().Select(t => t.Text));
            text = text.Replace('\u2028', '\n').TrimEnd('\n');
            Console.WriteLine($"{paragraphCount}\t{runCount}:\t'{text}'");
        }
    }
    // Basic text extraction: prints raw text in document body.
    var bodyText = body.InnerText;
    Console.WriteLine("----- Word Document Text -----");
    Console.WriteLine(string.IsNullOrWhiteSpace(bodyText) ? "<no text found>" : bodyText);
    Console.WriteLine("------------------------------");
}

static void ReadExcel(string file)
{
    using var doc = SpreadsheetDocument.Open(file, false);
    var workbookPart = doc.WorkbookPart;
    if (workbookPart == null || workbookPart.Workbook == null)
    {
        Console.WriteLine("No workbook found.");
        return;
    }

    // Build shared strings array for lookup
    var sharedStrings = workbookPart.SharedStringTablePart?.SharedStringTable?.Elements<SharedStringItem>()
                        .Select(ss => ss.InnerText).ToArray() ?? Array.Empty<string>();

    var sheet = workbookPart.Workbook.Sheets?.Elements<Sheet>().FirstOrDefault();
    if (sheet == null)
    {
        Console.WriteLine("No sheets found.");
        return;
    }

    var worksheetPart = (WorksheetPart)workbookPart.GetPartById(sheet.Id!);
    var rows = worksheetPart.Worksheet?.Elements<SheetData>().FirstOrDefault()?.Elements<Row>() ?? Enumerable.Empty<Row>();

    Console.WriteLine($"----- Excel Sheet: {sheet.Name} -----");
    foreach (var row in rows)
    {
        var values = row.Elements<Cell>().Select(cell =>
        {
            var v = cell.CellValue?.InnerText ?? string.Empty;
            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString && int.TryParse(v, out var idx))
                return idx >= 0 && idx < sharedStrings.Length ? sharedStrings[idx] : v;
            return v;
        });
        Console.WriteLine(string.Join("\t", values));
    }
    Console.WriteLine("------------------------------------");
}

static void ReadPowerPoint(string file)
{
    using var doc = PresentationDocument.Open(file, false);
    var presentationPart = doc.PresentationPart;
    if (presentationPart?.Presentation == null)
    {
        Console.WriteLine("No presentation found.");
        return;
    }

    var slideIds = presentationPart.Presentation.SlideIdList?.Elements<SlideId>() ?? Enumerable.Empty<SlideId>();
    Console.WriteLine("----- PowerPoint Slides -----");
    int idx = 1;
    foreach (var slideId in slideIds)
    {
        var slidePart = (SlidePart)presentationPart.GetPartById(slideId.RelationshipId!);
        // Try to get a slide title or fall back to first text
        var titleText = slidePart.Slide?.Descendants<DocumentFormat.OpenXml.Drawing.Text>()
                        .Select(t => t.Text).FirstOrDefault(t => !string.IsNullOrWhiteSpace(t));
        Console.WriteLine($"Slide {idx++}: {titleText ?? "<no text>"}");
    }
    Console.WriteLine("-----------------------------");
}
