using CodeMe.ScaffoldCS.Templates.Internals;

namespace CodeMe.ScaffoldCS.Templates.Redner.Symbols;

public readonly struct LiteralSymbol(string? value) : ITemplateValueSymbol
{
    public void Render(ITemplateWriter writer) => writer.WriteLiteral(value);
}