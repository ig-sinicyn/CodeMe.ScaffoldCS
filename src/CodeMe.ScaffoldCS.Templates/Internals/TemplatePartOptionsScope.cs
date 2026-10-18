using CodeMe.ScaffoldCS.Templates.Render.ValueSymbols;

namespace CodeMe.ScaffoldCS.Templates.Internals;

public readonly struct TemplatePartOptionsScope(ITemplatePartWriter owner, TemplateOptionsScopeEndSymbol scopeEndSymbol)
    : IDisposable
{
    public void Dispose() => owner.Write(scopeEndSymbol);
}