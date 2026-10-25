using CodeMe.ScaffoldCS.Templates.Internals;
using CodeMe.ScaffoldCS.Templates.Output;

namespace CodeMe.ScaffoldCS.Templates.Render.ValueSymbols;

public readonly struct TemplateBeginOptionsScopeSymbol(IndentationFormat indentation) : ITemplateValueSymbol
{
    public void Render(Template template) => template.BeginOptionsScope(indentation);
}