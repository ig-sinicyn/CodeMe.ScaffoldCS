using System.Runtime.InteropServices;

namespace CodeMe.ScaffoldCS.Templates.Infrastructure;

internal static class CollectionExtensions
{
    public static TValue GetOrAdd<TKey, TValue>(
        this Dictionary<TKey, TValue> dictionary,
        TKey key,
        Func<TKey, TValue> valueFactory)
        where TKey : notnull
    {
        ArgumentNullException.ThrowIfNull(dictionary);
        ArgumentNullException.ThrowIfNull(valueFactory);

        ref var value = ref CollectionsMarshal.GetValueRefOrAddDefault(dictionary, key, out var exists);
        if (!exists)
        {
            value = valueFactory(key);
        }

        return value!;
    }
}