using System.Globalization;

namespace CodeMe.ScaffoldCS.Templates.Output;

public sealed record TemplateTextWriterOptions(
    CultureInfo Culture,
    ReadOnlyMemory<char> Indentation,
    ReadOnlyMemory<char> SingleIndentation,
    NewLineFormat NewLineFormat,
    bool AppendIndentationOnEmptyLines,
    bool TrimLineEnd,
    bool NormalizeNewLines)
{
    public static readonly TemplateTextWriterOptions Default = new(
        Culture: CultureInfo.InvariantCulture,
        Indentation: Array.Empty<char>().AsMemory(),
        SingleIndentation: "\t".AsMemory(),
        NewLineFormat: NewLineFormat.Auto,
        AppendIndentationOnEmptyLines: true,
        TrimLineEnd: true,
        NormalizeNewLines: true);
}