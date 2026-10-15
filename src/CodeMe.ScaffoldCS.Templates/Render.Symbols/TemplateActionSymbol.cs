using CodeMe.ScaffoldCS.Templates.Internals;

namespace CodeMe.ScaffoldCS.Templates.Render.Parts;

public readonly struct TemplateActionSymbol(Action<Template> callback) : ITemplateSymbol
{
    public void Render(ITemplatePartWriter writer)
    {
        using var _ = writer.BeginTemplatePartScope();
        callback(writer);
    }
}