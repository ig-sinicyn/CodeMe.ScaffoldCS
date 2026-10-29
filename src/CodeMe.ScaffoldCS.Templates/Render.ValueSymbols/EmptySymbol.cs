using CodeMe.ScaffoldCS.Templates.Internals;

namespace CodeMe.ScaffoldCS.Templates.Render.ValueSymbols;

public readonly struct EmptySymbol : ITemplateValueSymbol
{
    public void Render(Template template)
    {
    }
}