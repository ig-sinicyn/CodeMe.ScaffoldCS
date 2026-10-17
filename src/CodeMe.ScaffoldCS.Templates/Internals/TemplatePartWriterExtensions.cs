using CodeMe.ScaffoldCS.Templates.Output;

namespace CodeMe.ScaffoldCS.Templates.Internals;

public static class TemplatePartWriterExtensions
{
    public static TemplateOptionsScope BeginOptionsScope(
        this ITemplatePartWriter writer,
        IndentationFormat indentationFormat)
    {
        var oldOptions = writer.Options;
        writer.Options = writer.CreateOptions(indentationFormat);
        return new TemplateOptionsScope(writer, oldOptions);
    }

    public static TemplateOptionsScope BeginOptionsScope(
        this ITemplatePartWriter writer,
        TemplateTextWriterOptions options)
    {
        var oldOptions = writer.Options;
        writer.Options = options;
        return new TemplateOptionsScope(writer, oldOptions);
    }

    public static TemplateOptionsScope BeginMultilineStringScope(this ITemplatePartWriter writer) =>
        writer.BeginOptionsScope(IndentationFormat.CurrentLine);
}