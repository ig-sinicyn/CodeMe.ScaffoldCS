using System.ComponentModel;
using System.Text;
using CodeMe.ScaffoldCS.Templates.Infrastructure;
using CodeMe.ScaffoldCS.Templates.Internals;
using CodeMe.ScaffoldCS.Templates.Output;
using CodeMe.ScaffoldCS.Templates.Render;
using CodeMe.ScaffoldCS.Templates.Render.Padding;
using CodeMe.ScaffoldCS.Templates.Render.ValueSymbols;

namespace CodeMe.ScaffoldCS.Templates;

public partial class Template
{
    private readonly Dictionary<string, PaddingCollection> _paddings = new();

    public TemplateTextWriterOptions CurrentOptions
    {
        get => _textWriter.Options;
        set => _textWriter.Options = value;
    }

    public IPaddingCollection? CurrentPadding { get; private set; }

    public bool IsBuffered { get; private set; }

    public TemplateOptionsScope BeginOptionsScope(IndentationFormat indentation)
    {
        if (IsBuffered)
        {
            var startSymbol = new TemplateOptionsScopeStartSymbol(indentation);
            Write(startSymbol);
            return new TemplateOptionsScope(this, x => x.Write(startSymbol.EndSymbol));
        }

        var oldOptions = _textWriter.Options;
        _textWriter.Options = _textWriter.CaptureOptions(indentation);
        return new TemplateOptionsScope(this, oldOptions);
    }

    internal void EndOptionsScope(TemplateTextWriterOptions optionsToRestore) => _textWriter.Options = optionsToRestore;

    public BufferingScope BeginBufferingScope()
    {
        var rootScope = StartBuffering();

        return new BufferingScope(this, rootScope);
    }

    public void EndBufferingScope(bool rootScope)
    {
        if (!rootScope)
        {
            return;
        }

        StopBuffering();
    }

    public PaddingScope BeginPaddingScope(PaddingRenderOptions? paddingOptions) =>
        BeginPaddingScope(Guid.NewGuid().ToString(), paddingOptions);

    public PaddingScope BeginPaddingScope(string name, PaddingRenderOptions? paddingOptions)
    {
        var oldPadding = CurrentPadding;
        CurrentPadding = _paddings.GetOrAdd(
            name,
            x => new PaddingCollection(x)
            {
                Options = paddingOptions ?? DefaultPaddingOptions
            });

        var rootScope = StartBuffering();
        return new PaddingScope(this, oldPadding, rootScope);
    }

    internal void EndPaddingScope(IPaddingCollection? paddingToRestore, bool stopBuffering)
    {
        CurrentPadding = paddingToRestore;
        if (stopBuffering)
        {
            StopBuffering();
        }
    }

    private bool StartBuffering()
    {
        if (IsBuffered)
        {
            return false;
        }

        IsBuffered = true;
        return true;
    }

    private void StopBuffering()
    {
        if (!IsBuffered)
        {
            return;
        }

        IsBuffered = false;

        if (_buffer.Count == 0)
        {
            return;
        }

        var estimateBuffer = new StringBuilder();
        foreach (var estimateSymbol in _buffer.OfType<IPaddingSymbol>())
        {
            if (estimateSymbol.Column is not { } column)
            {
                continue;
            }

            column.ObserveValueWidth(estimateSymbol.EstimateWidth(CurrentOptions.Culture, estimateBuffer));
        }

        foreach (var symbol in _buffer)
        {
            Write(symbol);
        }

        _buffer.Clear();
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public void BeginConditionBranch(ConditionBranch branch, bool condition) =>
        _branchState.BeginBranch(branch, condition);

    public void BeginIfBranch(bool condition) =>
        _branchState.BeginIfBranch(condition);

    public void BeginElseIfBranch(bool condition) =>
        _branchState.BeginElseIfBranch(condition);

    public void BeginElseBranch() =>
        _branchState.BeginElseBranch();

    public void CompleteIfBranch() =>
        _branchState.CompleteIfBranch();
}