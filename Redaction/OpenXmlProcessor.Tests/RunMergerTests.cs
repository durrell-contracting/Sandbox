using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using OpenXmlProcessor.OpenXml;
using OpenXmlProcessor.Services;
using System.Drawing;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using Color = DocumentFormat.OpenXml.Wordprocessing.Color;
using Text = DocumentFormat.OpenXml.Wordprocessing.Text;

namespace OpenXmlProcessor.Tests;

public class RunMergerTests
{
    private class FakeEncryptionService : IEncryptionService
    {
        public string Encrypt(string plainText) => $"ENC({plainText})";
        public string Decrypt(string encryptedText) => encryptedText[4..^1];
    }

    [Fact]
    public void CreateRedactionCustomXmlRun_EncryptsAndRenders_WithExpectedRunPropertiesAndText()
    {
        var svc = new FakeEncryptionService();
        var merger = new RunMerger(svc, renderWidth: 10);

        var element = XElement.Parse("<redact entity=\"NAME\">secret</redact>");
        var custom = merger.CreateRedactionCustomXmlRun(element, sourceProperties: null);

        // Expect two child runs: hidden(encrypted) and visible(rendered)
        Assert.Equal(2, custom.ChildElements.Count);

        var encryptedRun = Assert.IsType<Run>(custom.ChildElements[0]);
        var visibleRun = Assert.IsType<Run>(custom.ChildElements[1]);

        var encryptedText = encryptedRun.GetFirstChild<Text>()?.Text;
        var visibleText = visibleRun.GetFirstChild<Text>()?.Text;

        Assert.Equal("ENC(secret)", encryptedText);

        // With renderWidth=10 and entity "NAME" length=4 -> paddingLength = max(2, (10-4)/2)=3 -> "XXX NAME XXX"
        Assert.Equal("XXX NAME XXX", visibleText);

        // Encrypted run should have Hidden in its properties
        var encProps = encryptedRun.GetFirstChild<RunProperties>();
        Assert.NotNull(encProps?.GetFirstChild<Hidden>());

        // Visible run should have Bold, Color Val="000000" and Highlight black
        var visProps = visibleRun.GetFirstChild<RunProperties>();
        Assert.NotNull(visProps?.GetFirstChild<Bold>());
        var color = visProps?.GetFirstChild<Color>();
        Assert.Equal("000000", color?.Val?.Value);
        var highlight = visProps?.GetFirstChild<Highlight>();
        Assert.Equal(HighlightColorValues.Black, highlight?.Val?.Value);
    }

    [Fact]
    public void Merge_RedactionSpanningTwoTextRuns_ProducesPlainRedactionPlainSequence()
    {
        var svc = new FakeEncryptionService();
        var merger = new RunMerger(svc); // default renderWidth

        // Create two runs with Text children: "Hello" + "World" => combined "HelloWorld"
        var run1 = new Run(new RunProperties(new Italic()), new Text("Hello") { Space = SpaceProcessingModeValues.Preserve });
        var run2 = new Run(new RunProperties(new Underline { Val = UnderlineValues.Single }), new Text("World") { Space = SpaceProcessingModeValues.Preserve });

        var texts = new List<Text>
        {
            run1.GetFirstChild<Text>()!,
            run2.GetFirstChild<Text>()!
        };

        // Redaction spans end of first run and start of second: "loWo"
        var redactedElement = XElement.Parse("<r entity=\"NAME\">loWo</r>");
        var redactedElements = new List<XElement> { redactedElement };

        var result = merger.Merge(null, redactedElements, texts);

        // Expect three top-level children: plaintext Run("Hel"), CustomXmlRun(redaction), plaintext Run("rld")
        Assert.Equal(3, result.ChildElements.Count);

        var first = Assert.IsType<Run>(result.ChildElements[0]);
        var middle = Assert.IsType<CustomXmlRun>(result.ChildElements[1]);
        var last = Assert.IsType<Run>(result.ChildElements[2]);

        Assert.Equal("Hel", first.GetFirstChild<Text>()?.Text);
        Assert.Equal("rld", last.GetFirstChild<Text>()?.Text);

        // Middle should contain two inner runs (encrypted + rendered)
        Assert.Equal(2, middle.ChildElements.Count);
        var innerEncrypted = Assert.IsType<Run>(middle.ChildElements[0]);
        var innerVisible = Assert.IsType<Run>(middle.ChildElements[1]);

        Assert.Equal("ENC(loWo)", innerEncrypted.GetFirstChild<Text>()?.Text);
        // Rendered text uses entity attribute "NAME" -> with default renderWidth=10 results in "XXX NAME XXX"
        Assert.Equal("XXX NAME XXX", innerVisible.GetFirstChild<Text>()?.Text);
    }
}