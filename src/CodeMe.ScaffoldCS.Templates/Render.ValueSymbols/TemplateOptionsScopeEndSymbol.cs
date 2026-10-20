using CodeMe.ScaffoldCS.Templates.Internals;

namespace CodeMe.ScaffoldCS.Templates.Render.ValueSymbols;

public readonly struct TemplateOptionsScopeEndSymbol(TemplateOptionsScopeStartSymbol scopeStart) : ITemplateValueSymbol
{
    public void Render(Template template)
    {
        if (scopeStart.Scope is { } scope)
        {
            scope.Dispose();
            scopeStart.Scope = null;
        }
    }
}