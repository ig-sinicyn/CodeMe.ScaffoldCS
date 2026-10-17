using System.Text;
using CodeMe.ScaffoldCS.Templates.Internals;
using CodeMe.ScaffoldCS.Templates.Internals.Padding;

namespace CodeMe.ScaffoldCS.Templates.Render.PartSymbols;

public class PaddingSymbol<T>(T value, PaddingRenderOptions options) : IPaddingSymbol
{
    private readonly PaddingMode _autoMode = typeof(T) switch
    {
        var x when x == typeof(string) => PaddingMode.Left,
        var x when x == typeof(ReadOnlyMemory<char>) => PaddingMode.Left,
        var x when x == typeof(StringBuilder) => PaddingMode.Left,
        var x when x.IsAssignableTo(typeof(ITemplateValueSymbol)) => PaddingMode.None,
        var x when x.IsAssignableTo(typeof(IInterpolatedString)) => PaddingMode.None,
        _ => PaddingMode.Right
    };

    private int? _estimatedWidth;

    public IPaddingColumn? Column { get; set; }

    public void Render(ITemplatePartWriter writer)
    {
        var actualWidth = _estimatedWidth ?? EstimateWidth(writer.Options.Culture);
        var desiredWidth = Column?.ColumnWidth ?? actualWidth;

        RenderPadding(writer, GetPrefixWidth(desiredWidth, actualWidth));
        RenderCore(writer);
        RenderPadding(writer, GetSuffixWidth(desiredWidth, actualWidth));
    }

    private void RenderPadding(ITemplatePartWriter writer, int paddingWidth)
    {
        if (paddingWidth > 0)
        {
            Span<char> span = stackalloc char[paddingWidth];
            span.Fill(options.PaddingChar);
            writer.WriteLiteral(span);
        }
    }

    private void RenderCore(ITemplatePartWriter writer)
    {
        if (options.Format == null)
        {
            writer.Write(value);
        }
        else
        {
            writer.Write(value, 0, options.Format);
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
            .AppendFormatted(value, options.Format);

        var delta = buffer.Length - oldLength;
        var result = Math.Min(options.MinLength, delta);
        _estimatedWidth = result;
        return result;
    }
}