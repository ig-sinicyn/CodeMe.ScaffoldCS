using CodeMe.ScaffoldCS.Templates.Internals;

namespace CodeMe.ScaffoldCS.Templates.Render.ValueSymbols;

public readonly struct LiteralSymbol(ReadOnlyMemory<char> value) : ITemplateValueSymbol
{
    public LiteralSymbol(string? value) : this(value.AsMemory())
    {
    }

    public void Render(Template template) => template.WriteLiteral(value);
}