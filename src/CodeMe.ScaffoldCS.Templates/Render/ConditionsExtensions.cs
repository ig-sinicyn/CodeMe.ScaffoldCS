namespace CodeMe.ScaffoldCS.Templates.Render;

public static class ConditionsExtensions
{
    public static IEnumerable<T> WhereHasValue<T, TKey>(this IEnumerable<T> source) =>
        source.Where(x => Conditions.HasValue(x));

    public static IEnumerable<T> WhereHasValue<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector) =>
        source.Where(x => Conditions.HasValue(keySelector(x)));
}