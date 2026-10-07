using System.Diagnostics.CodeAnalysis;
using CodeMe.ScaffoldCS.Templates.Infrastructure;

namespace CodeMe.ScaffoldCS.Templates.Render;

public static class NameRender
{
    [return: NotNullIfNotNull(nameof(value))]
    public static string? TrimPrefix(
        this string? value,
        string prefix)
    {
        ArgumentNullException.ThrowIfNull(prefix);

        return string.IsNullOrEmpty(value) || prefix.Length == 0 || !value.StartsWith(prefix, StringComparison.Ordinal)
            ? value
            : value[prefix.Length..];
    }

    [return: NotNullIfNotNull(nameof(value))]
    public static string? TrimSuffix(
        this string? value,
        string suffix)
    {
        ArgumentNullException.ThrowIfNull(suffix);

        return string.IsNullOrEmpty(value) || suffix.Length == 0 || !value.EndsWith(suffix, StringComparison.Ordinal)
            ? value
            : value[.. ^suffix.Length];
    }

    [return: NotNullIfNotNull(nameof(value))]
    public static string? TrimPrefix(
        this string? value,
        params ReadOnlySpan<string> prefixes)
    {
        if (string.IsNullOrEmpty(value))
        {
            return value;
        }

        foreach (var prefix in prefixes)
        {
            if (prefix.Length > 0 && value.StartsWith(prefix, StringComparison.Ordinal))
            {
                return value[prefix.Length..];
            }
        }

        return value;
    }

    [return: NotNullIfNotNull(nameof(value))]
    public static string? TrimSuffix(
        this string? value,
        params ReadOnlySpan<string> suffixes)
    {
        if (string.IsNullOrEmpty(value))
        {
            return value;
        }

        foreach (var suffix in suffixes)
        {
            if (suffix.Length > 0 && value.EndsWith(suffix, StringComparison.Ordinal))
            {
                return value[..^suffix.Length];
            }
        }

        return value;
    }

    [return: NotNullIfNotNull(nameof(value))]
    public static string? TrimAllPrefixes(
        this string? value,
        params ReadOnlySpan<string> prefixes)
    {
        if (string.IsNullOrEmpty(value) || prefixes.Length == 0)
        {
            return value;
        }

        if (prefixes.Length == 1)
        {
            return value.TrimPrefix(prefixes[0]);
        }

        // Order by descending so prefixes with greater length will be checked first.
        // Soring is required to handle cases when we trim PrefixTestSuffix1 with prefixes (Pre, Prefix1).
        var prefixesToCheck = prefixes.ToArray().AsSpan();
        prefixesToCheck.Sort(StringComparer.Ordinal);
        prefixesToCheck.Reverse();

        var valueSpan = value.AsSpan();
        var replacedAny = true;
        while (valueSpan.Length > 0 && replacedAny)
        {
            replacedAny = false;
            for (var i = 0; i < prefixesToCheck.Length; i++)
            {
                var prefix = prefixesToCheck[i];
                if (prefix.Length > 0 && valueSpan.StartsWith(prefix, StringComparison.Ordinal))
                {
                    valueSpan = valueSpan[prefix.Length..];
                    prefixesToCheck[i] = "";
                    replacedAny = true;
                    break;
                }
            }
        }

        return valueSpan.ToString();
    }

    [return: NotNullIfNotNull(nameof(value))]
    public static string? TrimAllSuffixes(
        this string? value,
        params ReadOnlySpan<string> suffixes)
    {
        if (string.IsNullOrEmpty(value) || suffixes.Length == 0)
        {
            return value;
        }

        if (suffixes.Length == 1)
        {
            return value.TrimSuffix(suffixes[0]);
        }

        // HACK: we reverse suffixes and string and repeat behavior from TrimAllPrefixes.
        // Otherwise, we need to write custom comparer for sorting prefixes by its reversed values.
        // The hack is required to handle cases when we trim TestSuffix1 with suffixes (fix1, Suffix1, x1).
        var reversedSuffixes = new string[suffixes.Length].AsSpan();
        for (var i = 0; i < reversedSuffixes.Length; i++)
        {
            reversedSuffixes[i] = suffixes[i].ToReversedString();
        }

        reversedSuffixes.Sort();
        reversedSuffixes.Reverse();

        var reversedValueSpan = value.ToReversedString().AsSpan();
        var replacedAny = true;
        while (reversedValueSpan.Length > 0 && replacedAny)
        {
            replacedAny = false;
            for (var i = 0; i < reversedSuffixes.Length; i++)
            {
                var prefix = reversedSuffixes[i];
                if (prefix.Length > 0 && reversedValueSpan.StartsWith(prefix, StringComparison.Ordinal))
                {
                    reversedValueSpan = reversedValueSpan[prefix.Length..];
                    reversedSuffixes[i] = "";
                    replacedAny = true;
                    break;
                }
            }
        }

        return reversedValueSpan.ToReversedString();
    }

    public static string EnsurePrefix(this string? value, string prefix) => value switch
    {
        null => prefix,
        _ when value.StartsWith(prefix, StringComparison.Ordinal) => value,
        _ => prefix + value
    };

    public static string EnsureSuffix(this string? value, string suffix) => value switch
    {
        null => suffix,
        _ when value.EndsWith(suffix, StringComparison.Ordinal) => value,
        _ => value + suffix
    };
}