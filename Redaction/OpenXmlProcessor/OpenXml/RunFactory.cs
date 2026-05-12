using System;
using System.Xml.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;

namespace OpenXmlProcessor.OpenXml;

public static class RunFactory
{
    public static Run CreatePlaintextRun(string plaintext, RunProperties? properties = null)
    {
        if (plaintext is null) throw new ArgumentNullException(nameof(plaintext));
        return new Run(CopyRunProperties(properties), new Text(plaintext) { Space = SpaceProcessingModeValues.Preserve });
    }

    public static CustomXmlRun CreateRedactionCustomXmlRun(string encryptedText, string renderedText, RunProperties? sourceProperties = null)
    {
        if (encryptedText is null) throw new ArgumentNullException(nameof(encryptedText));
        if (renderedText is null) throw new ArgumentNullException(nameof(renderedText));

        var result = new CustomXmlRun();

        // Hidden encrypted run merges original style to have original style available during decryption
        var hiddenProps = MergeWithEncryption(sourceProperties);
        result.AppendChild(new Run(hiddenProps, new Text(encryptedText) { Space = SpaceProcessingModeValues.Preserve }));

        // Visible redaction run that merges original style where possible
        var visibleProps = MergeWithRedaction(sourceProperties);
        result.AppendChild(new Run(visibleProps, new Text(renderedText) { Space = SpaceProcessingModeValues.Preserve }));

        return result;
    }

    public static RunProperties CopyRunProperties(RunProperties? properties)
    {
        return properties?.CloneNode(true) as RunProperties ?? new RunProperties();
    }

    public static RunProperties CreateEncryptedRunProperties()
    {
        var result = new RunProperties();
        result.Append(new Hidden());
        return result;
    }

    public static RunProperties CreateRedactedRunProperties()
    {
        var result = new RunProperties();
        result.Append(new Bold());
        result.Append(new Color { Val = "000000" });
        result.Append(new Highlight { Val = HighlightColorValues.Black });
        return result;
    }

    public static RunProperties MergeWithEncryption(RunProperties? original)
    {
        var result = CopyRunProperties(original);
        // Add/override encryption styling
        result.Append(new Hidden());
        return result;
    }

    public static RunProperties MergeWithRedaction(RunProperties? original)
    {
        var result = CopyRunProperties(original);
        // Add/override redaction styling
        result.Append(new Bold());
        result.Append(new Color { Val = "000000" });
        result.Append(new Highlight { Val = HighlightColorValues.Black });
        return result;
    }
}