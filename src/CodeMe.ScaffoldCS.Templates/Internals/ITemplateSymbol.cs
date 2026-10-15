namespace CodeMe.ScaffoldCS.Templates.Internals;

public interface ITemplateSymbol
{
    void Render(ITemplatePartWriter writer);
}