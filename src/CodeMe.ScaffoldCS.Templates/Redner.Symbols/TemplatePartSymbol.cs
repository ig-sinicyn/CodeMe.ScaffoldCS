using CodeMe.ScaffoldCS.Templates.Internals;

namespace CodeMe.ScaffoldCS.Templates.Redner.Symbols;

public readonly struct TemplatePartSymbol(Func<TemplatePart> callback) : ITemplateSymbol
{
    public void Render(Template template)
    {
        using var _ = template.BeginTemplatePartScope();
        callback();
    }
}