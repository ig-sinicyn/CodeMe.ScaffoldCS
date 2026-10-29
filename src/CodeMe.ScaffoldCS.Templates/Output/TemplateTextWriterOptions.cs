using System.Globalization;

namespace CodeMe.ScaffoldCS.Templates.Output;

public sealed record TemplateTextWriterOptions(
    CultureInfo Culture,
    ReadOnlyMemory<char> Indentation,
    ReadOnlyMemory<char> SingleIndentation,
    NewLineFormat NewLineFormat,
    LastLineHandlingMode LastLineHandlingMode,
    bool AppendIndentationOnEmptyLines,
    bool TrimLineEnd,
    bool NormalizeNewLines)
{
    public static readonly TemplateTextWriterOptions Default = new(
        Culture: CultureInfo.InvariantCulture,
        Indentation: Array.Empty<char>().AsMemory(),
        SingleIndentation: "\t".AsMemory(),
        NewLineFormat: NewLineFormat.Auto,
        LastLineHandlingMode: LastLineHandlingMode.Normal,
        AppendIndentationOnEmptyLines: true,
        TrimLineEnd: true,
        NormalizeNewLines: true);
}