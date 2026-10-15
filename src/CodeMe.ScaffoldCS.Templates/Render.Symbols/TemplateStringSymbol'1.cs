using CodeMe.ScaffoldCS.Templates.Internals;

namespace CodeMe.ScaffoldCS.Templates.Render.Parts;

public readonly struct TemplateStringSymbol<TTemplatePart>(Func<TemplateString> callback) : ITemplateSymbol
    where TTemplatePart : IInterpolatedString
{
    public void Render(ITemplatePartWriter writer)
    {
        using var _ = writer.BeginTemplatePartScope();
        callback();
    }
}