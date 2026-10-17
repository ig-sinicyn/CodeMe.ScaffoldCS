using CodeMe.ScaffoldCS.Templates.Internals;

namespace CodeMe.ScaffoldCS.Templates.Render.PartSymbols;

public readonly struct TemplateActionSymbol(Action<ITemplatePartWriter> callback) : ITemplatePartSymbol
{
    public void Render(ITemplatePartWriter writer)
    {
        using var _ = writer.BeginMultilineStringScope();
        callback(writer);
    }
}