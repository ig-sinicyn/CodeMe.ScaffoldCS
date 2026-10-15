using CodeMe.ScaffoldCS.Templates.Internals;

namespace CodeMe.ScaffoldCS.Templates.Redner.Symbols;

public readonly struct ClearLineSymbol : ITemplateValueSymbol
{
    public void Render(ITemplateWriter writer) => writer.ClearLine();
}