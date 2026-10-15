using CodeMe.ScaffoldCS.Templates.Internals;

namespace CodeMe.ScaffoldCS.Templates.Redner.Symbols;

public readonly struct NewLineSymbol : ITemplateValueSymbol
{
    public void Render(ITemplateWriter writer) => writer.WriteLine();
}