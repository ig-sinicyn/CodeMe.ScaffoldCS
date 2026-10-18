using CodeMe.ScaffoldCS.Templates.Output;
using CodeMe.ScaffoldCS.Templates.Render.ValueSymbols;

namespace CodeMe.ScaffoldCS.Templates.Internals;

public static class TemplateWriterExtensions
{
    public static TemplateOptionsScope BeginMultilineStringScope(this ITemplateValueWriter writer) =>
        writer.BeginOptionsScope(IndentationFormat.CurrentLine);
}