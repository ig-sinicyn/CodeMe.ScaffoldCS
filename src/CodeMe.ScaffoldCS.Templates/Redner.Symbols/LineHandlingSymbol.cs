using CodeMe.ScaffoldCS.Templates.Internals;
using CodeMe.ScaffoldCS.Templates.Output;

namespace CodeMe.ScaffoldCS.Templates.Redner.Symbols;

public readonly struct LineHandlingSymbol(CurrentLineHandlingMode lineHandling) : ITemplateSymbol
{
    public void Render(Template template) => template.SetLineHandling(lineHandling);
}