using CodeMe.ScaffoldCS.Templates.Internals;

namespace CodeMe.ScaffoldCS.Templates.Render.PartSymbols;

public readonly struct TemplateStringSymbol<TTemplatePart>(Func<TemplateString> callback) : ITemplatePartSymbol
    where TTemplatePart : IInterpolatedString
{
    public void Render(ITemplatePartWriter writer)
    {
        using var _ = writer.BeginMultilineStringScope();
        callback();
    }
}