using CodeMe.ScaffoldCS.Templates.Internals;

namespace CodeMe.ScaffoldCS.Templates.Render.ValueSymbols;

public readonly struct LiteralSymbol(string? value) : ITemplateValueSymbol
{
    public void Render(ITemplateValueWriter writer) => writer.WriteLiteral(value);
}