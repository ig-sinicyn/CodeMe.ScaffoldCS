namespace CodeMe.ScaffoldCS.Templates.Internals;

public interface ITemplateScope
{
    void DisposeIfSame(Template caller);
}