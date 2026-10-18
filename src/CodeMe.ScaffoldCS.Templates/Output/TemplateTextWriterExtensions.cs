namespace CodeMe.ScaffoldCS.Templates.Output;

internal static class TemplateTextWriterExtensions
{
    public static TemplateTextWriterOptions CaptureOptions(this TemplateTextWriter writer, IndentationFormat format) =>
        format == IndentationFormat.CurrentIndentation
            ? writer.Options
            : writer.Options with { Indentation = writer.GetCurrentLineIndentation(format) };
}