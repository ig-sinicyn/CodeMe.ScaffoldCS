using System.ComponentModel;
using System.Text;
using CodeMe.ScaffoldCS.Templates.Infrastructure;
using CodeMe.ScaffoldCS.Templates.Internals;
using CodeMe.ScaffoldCS.Templates.Internals.Padding;
using CodeMe.ScaffoldCS.Templates.Output;
using CodeMe.ScaffoldCS.Templates.Render;
using CodeMe.ScaffoldCS.Templates.Render.ValueSymbols;

namespace CodeMe.ScaffoldCS.Templates;

public partial class Template
{
    private readonly Dictionary<string, PaddingGroup> _paddings = new();

    public TemplateTextWriterOptions CurrentOptions
    {
        get => _textWriter.Options;
        set => _textWriter.Options = value;
    }

    public bool IsBuffered { get; private set; }

    public IPaddingGroup? CurrentPadding { get; private set; }

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

    public TemplateBufferingScope BeginBufferingScope()
    {
        var bufferingStarted = StartBuffering();

        return new TemplateBufferingScope(this, stopBuffering: bufferingStarted);
    }

    public void EndBufferingScope(bool stopBuffering)
    {
        if (!stopBuffering)
        {
            return;
        }

        StopBuffering();
    }

    public TemplatePaddingScope BeginPaddingScope(PaddingRenderOptions? paddingOptions, string? name = null)
    {
        name ??= Guid.NewGuid().ToString();
        var oldPadding = CurrentPadding;
        CurrentPadding = _paddings.GetOrUpdate(
            name,
            x => new PaddingGroup(x, paddingOptions ?? DefaultPaddingOptions),
            (_, g) => g.Options = paddingOptions ?? g.Options);

        var bufferingStarted = StartBuffering();
        return new TemplatePaddingScope(this, oldPadding, stopBuffering: bufferingStarted);
    }

    internal void EndPaddingScope(IPaddingGroup? paddingToRestore, bool stopBuffering)
    {
        if (stopBuffering)
        {
            StopBuffering();
        }

        CurrentPadding = paddingToRestore;
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
        foreach (var symbol in _buffer.OfType<IPaddingSymbol>())
        {
            if (symbol.Column is not { } column)
            {
                continue;
            }

            column.ObserveValueWidth(symbol.EstimateWidth(CurrentOptions.Culture, estimateBuffer));
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