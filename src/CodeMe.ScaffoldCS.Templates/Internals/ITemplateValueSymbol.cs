namespace CodeMe.ScaffoldCS.Templates.Internals;

public interface ITemplateValueSymbol
{
    void Render(ITemplateValueWriter writer);
}