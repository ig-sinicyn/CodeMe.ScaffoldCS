using CodeMe.ScaffoldCS.Templates.Render.PartSymbols;

namespace CodeMe.ScaffoldCS.Templates.Render;

public static class PaddedCollectionRenderExtensions
{
    public static PaddedCollectionSymbol<T> RenderPadded<T>(
        this IEnumerable<T> value,
        PaddingRenderOptions? paddingOptions = null,
        string? paddingGroupName = null,
        CollectionRenderOptions? collectionOptions = null) =>
        new(
            source: value,
            callback: null,
            paddingOptions: paddingOptions,
            paddingGroupName: paddingGroupName,
            collectionOptions: collectionOptions);

    public static PaddedCollectionSymbol<T> RenderPadded<T>(
        this IEnumerable<T> value,
        Func<T, TemplateString> callback,
        PaddingRenderOptions? paddingOptions = null,
        string? paddingGroupName = null,
        CollectionRenderOptions? collectionOptions = null) =>
        new(
            source: value,
            callback: callback,
            paddingOptions: paddingOptions,
            paddingGroupName: paddingGroupName,
            collectionOptions: collectionOptions);

    public static PaddedCollectionSymbol<T> RenderPadded<T>(
        this IEnumerable<T> value,
        Action<Template, T> callback,
        PaddingRenderOptions? paddingOptions = null,
        string? paddingGroupName = null,
        CollectionRenderOptions? collectionOptions = null) =>
        new(
            source: value,
            callback: callback,
            paddingOptions: paddingOptions,
            paddingGroupName: paddingGroupName,
            collectionOptions: collectionOptions);
}