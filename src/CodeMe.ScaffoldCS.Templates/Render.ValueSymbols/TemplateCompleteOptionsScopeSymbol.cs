using CodeMe.ScaffoldCS.Templates.Internals;

namespace CodeMe.ScaffoldCS.Templates.Render.ValueSymbols;

public readonly struct TemplateCompleteOptionsScopeSymbol : ITemplateValueSymbol
{
    public void Render(Template template) => template.CompleteOptionsScope();
}