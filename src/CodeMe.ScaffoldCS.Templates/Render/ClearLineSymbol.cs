using CodeMe.ScaffoldCS.Templates.Internals;

namespace CodeMe.ScaffoldCS.Templates.Render;

public readonly struct ClearLineSymbol : ITemplateSymbol
{
    public void Render(Template template) => template.ClearLine();
}