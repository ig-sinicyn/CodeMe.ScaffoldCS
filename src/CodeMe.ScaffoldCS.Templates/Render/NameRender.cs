using System.Diagnostics.CodeAnalysis;
using System.Text;
using CodeMe.ScaffoldCS.Templates.Infrastructure;
using Humanizer;

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
            var suffix = suffixes[i];
            reversedSuffixes[i] = string.Create(
                suffix.Length, suffix, (chars, source) =>
                {
                    source.CopyTo(chars);
                    chars.Reverse();
                });
        }

        reversedSuffixes.Sort();
        reversedSuffixes.Reverse();

        var reversedValueSpan = value.ToArray().AsSpan();
        reversedValueSpan.Reverse();
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

        reversedValueSpan.Reverse();
        return reversedValueSpan.ToString();
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

    [return: NotNullIfNotNull(nameof(value))]
    public static string? ToSnakeCase(this string? value) => value.ToLowerWithSeparator("_");

    [return: NotNullIfNotNull(nameof(value))]
    public static string? ToSnakeUpperCase(this string? value) => value.ToUpperWithSeparator("_");

    [return: NotNullIfNotNull(nameof(value))]
    public static string? ToKebabCase(this string? value) => value.ToLowerWithSeparator("-");

    [return: NotNullIfNotNull(nameof(value))]
    public static string? ToKebabUpperCase(this string? value) => value.ToUpperWithSeparator("-");

    [return: NotNullIfNotNull(nameof(value))]
    public static string? ToLowerWithSeparator(this string? value, string separator)
    {
        if (value == null)
        {
            return null;
        }

        var result = new StringBuilder(value.Length);
        var prevIsLower = false;
        foreach (var c in value)
        {
            if (char.IsUpper(c))
            {
                if (prevIsLower)
                {
                    result.Append(separator);
                }

                result.Append(char.ToLowerInvariant(c));
                prevIsLower = false;
            }
            else
            {
                result.Append(c);
                prevIsLower = true;
            }
        }

        return result.ToString();
    }

    [return: NotNullIfNotNull(nameof(value))]
    public static string? ToUpperWithSeparator(this string? value, string separator)
    {
        if (value == null)
        {
            return null;
        }

        var result = new StringBuilder(value.Length);
        var prevIsLower = false;
        foreach (var c in value)
        {
            if (char.IsUpper(c))
            {
                if (prevIsLower)
                {
                    result.Append(separator);
                }

                result.Append(c);
                prevIsLower = false;
            }
            else
            {
                result.Append(char.ToUpperInvariant(c));
                prevIsLower = true;
            }
        }

        return result.ToString();
    }

    [return: NotNullIfNotNull(nameof(value))]
    public static string? ToPascalCase(this string? value) => value switch
    {
        null => null,
        _ when value.Length == 0 => value,
        _ when char.IsUpper(value[0]) => value,
        _ => char.ToUpperInvariant(value[0]) + value[1..]
    };

    [return: NotNullIfNotNull(nameof(value))]
    public static string? ToPascalCase(this string? value, params ReadOnlySpan<char> separators)
    {
        if (value == null)
        {
            return null;
        }

        if (value.Length == 0)
        {
            return value;
        }

        var result = new StringBuilder(value.Length);
        var span = value.AsSpan();
        var currentIndex = 0;
        while (currentIndex < span.Length)
        {
            var separatorIndex = span.NextIndexOfAny(currentIndex, separators);
            var nextIndex = separatorIndex < 0 ? span.Length : separatorIndex;
            if (nextIndex > currentIndex + 1)
            {
                result.Append(char.ToUpperInvariant(span[currentIndex]));
                result.Append(span[currentIndex..nextIndex]);
            }

            currentIndex = separatorIndex + 1;
        }

        return result.ToString();
    }

    // TODO: ToCamelCase with separators

    [return: NotNullIfNotNull(nameof(value))]
    public static string? ToCamelCase(this string? value)
    {
        if (value == null)
        {
            return null;
        }

        var result = new StringBuilder();

        var offset = 0;

        foreach (var c in value)
        {
            if (char.IsUpper(c))
            {
                offset++;
                result.Append(char.ToLowerInvariant(c));
            }
            else
            {
                break;
            }
        }

        if (offset < value.Length)
        {
            result.Append(value.AsSpan()[offset..]);
        }

        return result.ToString();
    }

    [return: NotNullIfNotNull(nameof(value))]
    public static string? ToSingular(this string? value) => value?.Singularize();

    [return: NotNullIfNotNull(nameof(value))]
    public static string? ToPlural(this string? value) => value?.Pluralize();

    [return: NotNullIfNotNull(nameof(value))]
    public static string? ToCamelSingularCase(this string? value) => value?.Singularize().ToCamelCase();

    [return: NotNullIfNotNull(nameof(value))]
    public static string? ToCamelPluralCase(this string? value) => value?.Pluralize().ToCamelCase();
}