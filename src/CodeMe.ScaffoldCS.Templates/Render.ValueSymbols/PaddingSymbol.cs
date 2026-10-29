using System.Text;
using CodeMe.ScaffoldCS.Templates.Internals;

namespace CodeMe.ScaffoldCS.Templates.Render.ValueSymbols;

public class PaddingSymbol<T> : IPaddingSymbol
{
    private readonly T _value;
    private readonly string? _format;
    private readonly PaddingMode _padding;
    private readonly char _paddingChar;
    private readonly int _minLength;

    private int? _actualWidth;

    public PaddingSymbol(T value, PaddingRenderOptions options, int alignment = 0, string? format = null)
    {
        _value = value;
        _format = format;
        _padding = options.Padding == PaddingMode.Auto
            ? typeof(T) switch
            {
                _ when alignment < 0 => PaddingMode.Left,
                _ when alignment > 0 => PaddingMode.Right,
                var x when x == typeof(string) => PaddingMode.Left,
                var x when x == typeof(ReadOnlyMemory<char>) => PaddingMode.Left,
                var x when x == typeof(StringBuilder) => PaddingMode.Left,
                var x when x.IsAssignableTo(typeof(ITemplateSymbol)) => PaddingMode.None,
                var x when x.IsAssignableTo(typeof(IInterpolatedString)) => PaddingMode.None,
                _ => PaddingMode.Right
            }
            : options.Padding;
        _paddingChar = options.PaddingChar;
        _minLength = Math.Max(Math.Abs(alignment), options.MinLength);
    }

    public PaddingColumn? Column { get; set; }

    public void Render(Template template)
    {
        var padWidth = Math.Max(_minLength, Column?.Width ?? 0);
        var actual = GetActualWidth(template.CurrentOptions.Culture);

        // TODO: multiline scope
        RenderPadding(template, GetPrefixWidth(padWidth, actual));
        RenderCore(template);
        RenderPadding(template, GetSuffixWidth(padWidth, actual));
    }

    private int GetPrefixWidth(int padWidth, int actual) => _padding switch
    {
        PaddingMode.None => 0,
        PaddingMode.Auto => 0,
        PaddingMode.Left => 0,
        PaddingMode.Right => Math.Max(0, padWidth - actual),
        PaddingMode.Center => Math.Max(0, padWidth / 2 - actual),
        _ => throw new InvalidOperationException($"Unknown padding value {_padding}")
    };

    private int GetSuffixWidth(int padWidth, int actual) => _padding switch
    {
        PaddingMode.None => 0,
        PaddingMode.Auto => 0,
        PaddingMode.Left => Math.Max(0, padWidth - actual),
        PaddingMode.Right => 0,
        PaddingMode.Center => Math.Max(0, (padWidth + 1) / 2 - actual),
        _ => throw new InvalidOperationException($"Unknown padding value {_padding}")
    };

    private void RenderPadding(Template template, int paddingWidth)
    {
        if (paddingWidth > 0)
        {
            Span<char> span = stackalloc char[paddingWidth];
            span.Fill(_paddingChar);
            template.WriteLiteral(span);
        }
    }

    private void RenderCore(Template template)
    {
        if (_format != null)
        {
            template.Write(_value, 0, _format);
        }
        else
        {
            template.Write(_value);
        }
    }

    public int EstimateWidth(IFormatProvider? formatProvider, StringBuilder? buffer = null) =>
        Math.Max(_minLength, GetActualWidth(formatProvider, buffer));

    public int GetActualWidth(IFormatProvider? formatProvider, StringBuilder? buffer = null)
    {
        if (_actualWidth is { } width)
        {
            return width;
        }

        if (_padding == PaddingMode.None)
        {
            _actualWidth = 0;
            return 0;
        }

        buffer ??= new StringBuilder();

        var oldLength = buffer.Length;
        new StringBuilder.AppendInterpolatedStringHandler(0, 0, buffer, formatProvider)
            .AppendFormatted(_value, _format);

        var result = buffer.Length - oldLength;
        _actualWidth = result;
        return result;
    }
}