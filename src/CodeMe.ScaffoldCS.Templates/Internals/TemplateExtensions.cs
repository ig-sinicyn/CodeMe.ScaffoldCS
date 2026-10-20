using CodeMe.ScaffoldCS.Templates.Output;

namespace CodeMe.ScaffoldCS.Templates.Internals;

public static class TemplateExtensions
{
    public static TemplateOptionsScope BeginMultilineStringScope(this Template writer) =>
        writer.BeginOptionsScope(IndentationFormat.CurrentLine);
}