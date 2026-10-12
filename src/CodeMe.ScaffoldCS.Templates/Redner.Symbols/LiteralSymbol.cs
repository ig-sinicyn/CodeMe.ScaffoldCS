using CodeMe.ScaffoldCS.Templates.Internals;

namespace CodeMe.ScaffoldCS.Templates.Redner.Symbols;

public readonly struct LiteralSymbol(string? value) : ITemplateSymbol
{
    public void Render(Template template) => template.WriteLiteral(value);
}