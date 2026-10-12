using CodeMe.ScaffoldCS.Templates.Internals;

namespace CodeMe.ScaffoldCS.Templates.Redner.Symbols;

public readonly struct NewLineSymbol : ITemplateSymbol
{
    public void Render(Template template) => template.WriteLine();
}