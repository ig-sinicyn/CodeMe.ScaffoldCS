using System.Collections;

namespace CodeMe.ScaffoldCS.Templates.Render;

public static class Conditions
{
    public static bool HasValue<T>(T? value) =>
        value switch
        {
            null => false,
            string s => !string.IsNullOrEmpty(s),
            ICollection collection => collection.Count > 0,
            IEnumerable enumerable => HasValue(enumerable),
            _ => !EqualityComparer<T>.Default.Equals(value, default!)
        };

    public static bool HasValue<T>(ReadOnlySpan<T> value) => value.Length > 0;

    public static bool HasValue<T>(ReadOnlyMemory<T> value) => value.Length > 0;

    public static bool HasValue<T>(IReadOnlyCollection<T> value) => value.Count > 0;

    public static bool HasValue<TKey, TValue>(IReadOnlyDictionary<TKey, TValue> value) => value.Count > 0;

    private static bool HasValue(IEnumerable enumerable)
    {
        var enumerator = enumerable.GetEnumerator();
        try
        {
            return enumerator.MoveNext();
        }
        finally
        {
            (enumerator as IDisposable)?.Dispose();
        }
    }
}