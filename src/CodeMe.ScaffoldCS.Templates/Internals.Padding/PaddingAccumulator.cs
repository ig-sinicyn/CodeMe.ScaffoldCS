using System.Text;
using CodeMe.ScaffoldCS.Templates.Render;

namespace CodeMe.ScaffoldCS.Templates.Internals.Padding;

public class PaddingAccumulator(Template writer)
{
    private readonly StringBuilder _buffer = new();
    private readonly List<PaddingColumn> _columns = new();
    private readonly List<ITemplateValueSymbol> _deferredSymbols = new();
    private int _columnIndex = -1;

    public PaddingRenderOptions DefaultPaddingRenderOptions { get; set; } = PaddingRenderOptions.Default;

    public void NextRow() => _columnIndex = -1;

    private PaddingColumn NextColumn()
    {
        _columnIndex++;

        while (_columns.Count >= _columnIndex)
        {
            _columns.Add(new PaddingColumn());
        }

        return _columns[_columnIndex];
    }

    public IDisposable BeginAmbientScope() => AmbientPaddingAccumulator.BeginAccumulatorScope(this);

    public void Write(ITemplateValueSymbol symbol)
    {
        if (symbol is IPaddingSymbol paddingSymbol)
        {
            _buffer.Clear();
            try
            {
                var valueWidth = paddingSymbol.EstimateWidth(writer.Options.Culture, _buffer);

                var column = NextColumn();
                column.ColumnWidth = Math.Max(column.ColumnWidth, valueWidth);

                paddingSymbol.Column = column;
            }
            finally
            {
                _buffer.Clear();
            }
        }

        _deferredSymbols.Add(symbol);
    }

    public void DisposeIfSame(Template caller)
    {
        if (ReferenceEquals(writer, caller))
        {
            foreach (var deferredSymbol in _deferredSymbols)
            {
                writer.Write(deferredSymbol);
            }

            _deferredSymbols.Clear();
            _columns.Clear();
        }
    }
}