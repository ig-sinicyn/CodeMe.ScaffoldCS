using CodeMe.ScaffoldCS.Templates.Internals;
using CodeMe.ScaffoldCS.Templates.Output;

namespace CodeMe.ScaffoldCS.Templates.Render.ValueSymbols;

public readonly struct LineHandlingSymbol(CurrentLineHandlingMode lineHandling) : ITemplateValueSymbol
{
    public void Render(Template template) => template.SetLineHandling(lineHandling);
}