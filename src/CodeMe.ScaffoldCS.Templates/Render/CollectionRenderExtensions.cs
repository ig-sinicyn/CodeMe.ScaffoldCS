using CodeMe.ScaffoldCS.Templates.Render.PartSymbols;

namespace CodeMe.ScaffoldCS.Templates.Render;

public static class CollectionRenderExtensions
{
    public static CollectionSymbol<T> Render<T>(
        this IEnumerable<T> value,
        CollectionRenderOptions? options = null) =>
        new(value, null, options);

    public static CollectionSymbol<T> Render<T>(
        this IEnumerable<T> value,
        Func<T, TemplateString> callback,
        CollectionRenderOptions? options = null) =>
        new(value, callback, options);

    public static CollectionSymbol<T> Render<T>(
        this IEnumerable<T> value,
        Action<Template, T> callback,
        CollectionRenderOptions? options = null) =>
        new(value, callback, options);

    public static CollectionSymbol<T> RenderCommaSeparated<T>(this IEnumerable<T> value) =>
        new(value, null, CollectionRenderOptions.CommaSeparated);

    public static CollectionSymbol<T> RenderCommaSeparated<T>(
        this IEnumerable<T> value,
        Func<T, TemplateString> callback) =>
        new(value, callback, CollectionRenderOptions.CommaSeparated);

    public static CollectionSymbol<T> RenderEmptyLineSeparated<T>(
        this IEnumerable<T> value) =>
        new(value, null, CollectionRenderOptions.EmptyLineSeparated);

    public static CollectionSymbol<T> RenderEmptyLineSeparated<T>(
        this IEnumerable<T> value,
        Func<T, TemplateString> callback) =>
        new(value, callback, CollectionRenderOptions.EmptyLineSeparated);
}