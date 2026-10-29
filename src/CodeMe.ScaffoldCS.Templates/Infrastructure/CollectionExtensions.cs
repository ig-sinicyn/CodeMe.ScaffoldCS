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

    public static TValue GetOrUpdate<TKey, TValue>(
        this Dictionary<TKey, TValue> dictionary,
        TKey key,
        Func<TKey, TValue> newValueFactory,
        Func<TKey, TValue, TValue> updatedValueFactory)
        where TKey : notnull
    {
        ArgumentNullException.ThrowIfNull(dictionary);
        ArgumentNullException.ThrowIfNull(newValueFactory);
        ArgumentNullException.ThrowIfNull(updatedValueFactory);

        ref var value = ref CollectionsMarshal.GetValueRefOrAddDefault(dictionary, key, out var exists);
        if (exists)
        {
            value = updatedValueFactory(key, value!);
        }
        else
        {
            value = newValueFactory(key);
        }

        return value!;
    }

    public static TValue GetOrUpdate<TKey, TValue>(
        this Dictionary<TKey, TValue> dictionary,
        TKey key,
        Func<TKey, TValue> newValueFactory,
        Action<TKey, TValue> updatedValueCallback)
        where TKey : notnull
    {
        ArgumentNullException.ThrowIfNull(dictionary);
        ArgumentNullException.ThrowIfNull(newValueFactory);
        ArgumentNullException.ThrowIfNull(updatedValueCallback);

        ref var value = ref CollectionsMarshal.GetValueRefOrAddDefault(dictionary, key, out var exists);
        if (exists)
        {
            updatedValueCallback(key, value!);
        }
        else
        {
            value = newValueFactory(key);
        }

        return value!;
    }
}