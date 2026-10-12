using CodeMe.ScaffoldCS.Templates.Internals;

namespace CodeMe.ScaffoldCS.Templates.Redner.Symbols;

public readonly struct TemplatePartSymbol<TTemplatePart>(Func<TemplatePart> callback) : ITemplateSymbol
    where TTemplatePart : ITemplatePart
{
    public void Render(Template template)
    {
        using var _ = template.BeginTemplatePartScope();
        callback();
    }
}