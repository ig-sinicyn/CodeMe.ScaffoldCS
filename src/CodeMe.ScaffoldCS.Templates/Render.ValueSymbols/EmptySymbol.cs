using CodeMe.ScaffoldCS.Templates.Internals;

namespace CodeMe.ScaffoldCS.Templates.Redner.Symbols;

public readonly struct EmptySymbol : ITemplateValueSymbol
{
    public void Render(ITemplateWriter writer)
    {
    }
}