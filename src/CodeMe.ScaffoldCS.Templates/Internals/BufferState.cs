using CodeMe.ScaffoldCS.Templates.Infrastructure;
using CodeMe.ScaffoldCS.Templates.Render;

namespace CodeMe.ScaffoldCS.Templates.Internals;

public class BufferState
{
    private readonly Dictionary<string, List<PaddingColumn>> _paddingColumns = new();

    private readonly Stack<PaddingGroup?> _paddingStack = new();

    private readonly List<ITemplateValueSymbol> _bufferedSymbols = new();

    public bool IsEmpty => _paddingStack.Count == 0;

    public PaddingGroup? CurrentPadding { get; private set; }

    public bool ShouldBuffer => _paddingStack.Count > 0;

    public void Clear()
    {
        _paddingColumns.Clear();
        _paddingStack.Clear();
        _bufferedSymbols.Clear();
    }

    public void BeginScope()
    {
        _paddingStack.Push(CurrentPadding);
        CurrentPadding = null;
    }

    public void BeginPaddingScope(string name, PaddingRenderOptions options)
    {
        var sharedColumns = _paddingColumns.GetOrAdd(name, x => new List<PaddingColumn>());
        var newPadding = new PaddingGroup(name, options, sharedColumns);

        _paddingStack.Push(CurrentPadding);
        CurrentPadding = newPadding;
    }

    public void CompletePaddingScope(Action<IReadOnlyCollection<ITemplateValueSymbol>> consumeBufferCallback)
    {
        CurrentPadding = _paddingStack.Pop();
        if (_paddingStack.Count == 0 && _bufferedSymbols.Count > 0)
        {
            consumeBufferCallback(_bufferedSymbols);
            _bufferedSymbols.Clear();
        }
    }

    public void Append(ITemplateValueSymbol symbol)
    {
        if (!ShouldBuffer)
        {
            throw new InvalidOperationException("Start buffering scope before appending to the buffer");
        }

        _bufferedSymbols.Add(symbol);
    }
}