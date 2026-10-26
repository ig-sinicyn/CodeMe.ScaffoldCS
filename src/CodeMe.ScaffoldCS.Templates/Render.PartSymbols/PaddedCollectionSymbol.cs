using CodeMe.ScaffoldCS.Templates.Internals;

namespace CodeMe.ScaffoldCS.Templates.Render.PartSymbols;

public readonly struct PaddedCollectionSymbol<T> : ITemplateSymbol
{
    private readonly CollectionSymbolPart<T> _wrappedSymbol;
    private readonly PaddingRenderOptions? _paddingOptions;
    private readonly string? _paddingGroupName;

    public PaddedCollectionSymbol(
        IEnumerable<T> source,
        Func<T, TemplateString>? callback = null,
        PaddingRenderOptions? paddingOptions = null,
        string? paddingGroupName = null,
        CollectionRenderOptions? collectionOptions = null)
    {
        _wrappedSymbol = new CollectionSymbolPart<T>(source, callback, collectionOptions);
        _paddingOptions = paddingOptions;
        _paddingGroupName = paddingGroupName;
    }

    public PaddedCollectionSymbol(
        IEnumerable<T> source,
        Action<Template, T> callback,
        PaddingRenderOptions? paddingOptions = null,
        string? paddingGroupName = null,
        CollectionRenderOptions? collectionOptions = null)
    {
        _wrappedSymbol = new CollectionSymbolPart<T>(source, callback, collectionOptions);
        _paddingOptions = paddingOptions;
        _paddingGroupName = paddingGroupName;
    }

    public void Render(Template template)
    {
        using var _ = template.BeginPaddingScope(_paddingOptions, _paddingGroupName);
        _wrappedSymbol.Render(template);
    }
}