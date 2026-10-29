using CodeMe.ScaffoldCS.Templates.Internals;
using CodeMe.ScaffoldCS.Templates.Output;

namespace CodeMe.ScaffoldCS.Templates.Render.ValueSymbols;

public readonly struct TemplateOptionsSymbol(TemplateTextWriterOptions options) : ITemplateValueSymbol
{
    public void Render(Template template) => template.CurrentOptions = options;
}