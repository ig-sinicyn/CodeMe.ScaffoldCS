using CodeMe.ScaffoldCS.Templates.Internals;
using CodeMe.ScaffoldCS.Templates.Output;

namespace CodeMe.ScaffoldCS.Templates.Redner.Symbols;

public readonly struct LineHandlingSymbol(CurrentLineHandlingMode lineHandling) : ITemplateValueSymbol
{
    public void Render(ITemplateWriter writer) => writer.SetLineHandling(lineHandling);
}