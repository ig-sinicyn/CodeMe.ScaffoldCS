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

        return value == null || !value.StartsWith(prefix, StringComparison.Ordinal)
            ? value
            : value[prefix.Length..];
    }

    [return: NotNullIfNotNull(nameof(value))]
    public static string? TrimSuffix(
        this string? value,
        string suffix)
    {
        ArgumentNullException.ThrowIfNull(suffix);

        return value == null || !value.EndsWith(suffix, StringComparison.Ordinal)
            ? value
            : value[.. ^suffix.Length];
    }

    [return: NotNullIfNotNull(nameof(value))]
    public static string? TrimPrefixes(
        this string? value,
        params Span<string> prefixes)
    {
        if (string.IsNullOrEmpty(value))
        {
            return value;
        }

        foreach (var prefix in prefixes)
        {
            if (value.StartsWith(prefix, StringComparison.Ordinal))
            {
                value = value[prefix.Length..];
            }
        }

        return value;
    }

    [return: NotNullIfNotNull(nameof(value))]
    public static string? TrimSuffixes(
        this string? value,
        params Span<string> suffixes)
    {
        if (string.IsNullOrEmpty(value))
        {
            return value;
        }

        foreach (var prefix in suffixes)
        {
            if (value.EndsWith(prefix, StringComparison.Ordinal))
            {
                value = value[.. ^prefix.Length];
            }
        }

        return value;
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
    public static string? ToPascalCase(this string? value, params Span<char> separators)
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