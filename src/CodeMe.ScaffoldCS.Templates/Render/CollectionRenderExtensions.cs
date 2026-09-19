using CodeMe.ScaffoldCS.Templates.Internals;

namespace CodeMe.ScaffoldCS.Templates.Render;

public static class CollectionRenderExtensions
{
    public static ITemplateSymbol Render<T>(this IEnumerable<T> value, CollectionRenderOptions? options = null) =>
        new CollectionSymbol<T>(value, null, options ?? CollectionRenderOptions.Default);

    public static ITemplateSymbol Render<T>(
        this IEnumerable<T> value,
        Func<T, TemplatePart> format,
        CollectionRenderOptions? options = null) =>
        new CollectionSymbol<T>(value, format, options ?? CollectionRenderOptions.Default);

    public static ITemplateSymbol RenderCommaSeparated<T>(this IEnumerable<T> value) =>
        new CollectionSymbol<T>(value, null, CollectionRenderOptions.CommaSeparated);

    public static ITemplateSymbol RenderEmptyLineSeparated<T>(this IEnumerable<T> value) =>
        new CollectionSymbol<T>(value, null, CollectionRenderOptions.EmptyLineSeparated);

    public static ITemplateSymbol RenderEmptyLineSeparated<T>(
        this IEnumerable<T> value,
        Func<T, TemplatePart> format) =>
        new CollectionSymbol<T>(value, format, CollectionRenderOptions.EmptyLineSeparated);
}