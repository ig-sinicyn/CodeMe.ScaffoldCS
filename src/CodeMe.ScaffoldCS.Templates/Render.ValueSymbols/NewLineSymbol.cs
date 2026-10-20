using CodeMe.ScaffoldCS.Templates.Internals;

namespace CodeMe.ScaffoldCS.Templates.Render.ValueSymbols;

public readonly struct NewLineSymbol : ITemplateValueSymbol
{
    public void Render(Template template) => template.WriteLine();
}