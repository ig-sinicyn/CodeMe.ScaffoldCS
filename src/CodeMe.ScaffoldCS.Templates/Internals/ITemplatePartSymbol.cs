namespace CodeMe.ScaffoldCS.Templates.Internals;

public interface ITemplatePartSymbol
{
    void Render(ITemplatePartWriter writer);
}