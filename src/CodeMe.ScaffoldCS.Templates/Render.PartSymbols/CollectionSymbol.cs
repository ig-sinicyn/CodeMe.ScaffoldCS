using CodeMe.ScaffoldCS.Templates.Internals;

namespace CodeMe.ScaffoldCS.Templates.Render.PartSymbols;

public readonly struct CollectionSymbol<T> : ITemplateSymbol
{
    private readonly CollectionSymbolPart<T> _wrappedSymbol;

    public CollectionSymbol(
        IEnumerable<T> source,
        Func<T, TemplateString>? callback,
        CollectionRenderOptions? options)
    {
        _wrappedSymbol = new CollectionSymbolPart<T>(source, callback, options);
    }

    public CollectionSymbol(
        IEnumerable<T> source,
        Action<Template, T> callback,
        CollectionRenderOptions? options)
    {
        _wrappedSymbol = new CollectionSymbolPart<T>(source, callback, options);
    }

    public void Render(Template template)
    {
        // Disable current padding if there is one
        if (template.CurrentPadding != null)
        {
            using var _ = template.BeginBufferingScope();
            _wrappedSymbol.Render(template);
            return;
        }

        _wrappedSymbol.Render(template);
    }
}