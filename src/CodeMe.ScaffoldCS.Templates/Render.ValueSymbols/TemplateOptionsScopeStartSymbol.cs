using CodeMe.ScaffoldCS.Templates.Internals;
using CodeMe.ScaffoldCS.Templates.Output;

namespace CodeMe.ScaffoldCS.Templates.Render.ValueSymbols;

public class TemplateOptionsScopeStartSymbol(IndentationFormat indentation) : ITemplateValueSymbol
{
    public TemplateOptionsScope? Scope { get; internal set; }

    public void Render(ITemplateValueWriter writer) => Scope = writer.BeginOptionsScope(indentation);

    public TemplateOptionsScopeEndSymbol EndSymbol => new(this);
}