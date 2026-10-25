using System.Text;
using CodeMe.ScaffoldCS.Templates.Internals;

namespace CodeMe.ScaffoldCS.Templates.Render.ValueSymbols;

public class PaddingSymbol<T>(T value, PaddingRenderOptions options, int alignment = 0, string? format = null)
    : IPaddingSymbol
{
    private readonly PaddingMode _autoMode = typeof(T) switch
    {
        var x when x == typeof(string) => PaddingMode.Left,
        var x when x == typeof(ReadOnlyMemory<char>) => PaddingMode.Left,
        var x when x == typeof(StringBuilder) => PaddingMode.Left,
        var x when x.IsAssignableTo(typeof(ITemplateSymbol)) => PaddingMode.None,
        var x when x.IsAssignableTo(typeof(IInterpolatedString)) => PaddingMode.None,
        _ => PaddingMode.Right
    };

    private int? _estimatedWidth;

    public PaddingColumn? Column { get; set; }

    public void Render(Template template)
    {
        var actualWidth = _estimatedWidth ?? EstimateWidth(template.CurrentOptions.Culture);
        var desiredWidth = Column?.Width ?? actualWidth;

        // TODO: multiline scope
        RenderPadding(template, GetPrefixWidth(desiredWidth, actualWidth));
        RenderCore(template);
        RenderPadding(template, GetSuffixWidth(desiredWidth, actualWidth));
    }

    private void RenderPadding(Template template, int paddingWidth)
    {
        if (paddingWidth > 0)
        {
            Span<char> span = stackalloc char[paddingWidth];
            span.Fill(options.PaddingChar);
            template.WriteLiteral(span);
        }
    }

    private void RenderCore(Template template)
    {
        var hasFormat = alignment != 0 || format != null;
        if (hasFormat)
        {
            template.Write(value, alignment, format);
        }
        else
        {
            template.Write(value);
        }
    }

    private int GetPrefixWidth(int desired, int actual)
    {
        var resolvedPadding = options.Padding == PaddingMode.Auto ? _autoMode : options.Padding;
        return resolvedPadding switch
        {
            PaddingMode.None => 0,
            PaddingMode.Auto => 0,
            PaddingMode.Left => 0,
            PaddingMode.Right => Math.Max(0, desired - actual),
            PaddingMode.Center => Math.Max(0, desired / 2 - actual),
            _ => throw new ArgumentOutOfRangeException(nameof(options.Padding), options.Padding, null)
        };
    }

    private int GetSuffixWidth(int desired, int actual)
    {
        var resolvedPadding = options.Padding == PaddingMode.Auto ? _autoMode : options.Padding;
        return resolvedPadding switch
        {
            PaddingMode.None => 0,
            PaddingMode.Auto => 0,
            PaddingMode.Left => Math.Max(0, desired - actual),
            PaddingMode.Right => 0,
            PaddingMode.Center => Math.Max(0, (desired + 1) / 2 - actual),
            _ => throw new ArgumentOutOfRangeException(nameof(options.Padding), options.Padding, null)
        };
    }

    public int EstimateWidth(IFormatProvider? formatProvider, StringBuilder? buffer = null)
    {
        var resolvedPadding = options.Padding == PaddingMode.Auto ? _autoMode : options.Padding;
        if (resolvedPadding == PaddingMode.None)
        {
            return 0;
        }

        buffer ??= new StringBuilder();

        var oldLength = buffer.Length;
        new StringBuilder.AppendInterpolatedStringHandler(0, 0, buffer, formatProvider)
            .AppendFormatted(value, format);

        var delta = buffer.Length - oldLength;
        var result = Math.Max(options.MinLength, delta);
        _estimatedWidth = result;
        return result;
    }
}