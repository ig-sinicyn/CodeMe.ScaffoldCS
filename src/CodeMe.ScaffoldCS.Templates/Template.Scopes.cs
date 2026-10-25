using System.ComponentModel;
using System.Text;
using CodeMe.ScaffoldCS.Templates.Internals;
using CodeMe.ScaffoldCS.Templates.Output;
using CodeMe.ScaffoldCS.Templates.Render;
using CodeMe.ScaffoldCS.Templates.Render.ValueSymbols;

namespace CodeMe.ScaffoldCS.Templates;

public partial class Template
{
    public TemplateTextWriterOptions CurrentOptions
    {
        get => _optionsState.Options;
        set
        {
            _optionsState.Options = value;
            _textWriter.Options = value;
        }
    }

    public bool ShouldWrite => _branchState.ShouldWrite;

    public bool ShouldBuffer => _bufferState.ShouldBuffer;

    public PaddingGroup? CurrentPadding => _bufferState.CurrentPadding;

    public TemplateOptionsScope BeginOptionsScope(IndentationFormat indentation)
    {
        if (ShouldBuffer)
        {
            Write(new TemplateBeginOptionsScopeSymbol(indentation));
            return new TemplateOptionsScope(this);
        }

        _optionsState.BeginOptionsScope(_textWriter.CaptureOptions(indentation));
        _textWriter.Options = _optionsState.Options;
        return new TemplateOptionsScope(this);
    }

    internal void CompleteOptionsScope()
    {
        if (ShouldBuffer)
        {
            Write(new TemplateCompleteOptionsScopeSymbol());
            return;
        }

        _optionsState.CompleteOptionsScope();
        _textWriter.Options = _optionsState.Options;
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

    public TemplateBufferingScope BeginBufferingScope()
    {
        _bufferState.BeginScope();
        return new TemplateBufferingScope(this);
    }

    public TemplateBufferingScope BeginPaddingScope(PaddingRenderOptions? paddingOptions = null, string? name = null)
    {
        _bufferState.BeginPaddingScope(
            name ?? Guid.NewGuid().ToString(),
            paddingOptions ?? DefaultPaddingOptions);
        return new TemplateBufferingScope(this);
    }

    internal void CompleteBufferingScope() => _bufferState.CompletePaddingScope(x => ConsumeBuffer(x));

    private void ConsumeBuffer(IReadOnlyCollection<ITemplateValueSymbol> bufferedSymbols)
    {
        var estimateBuffer = new StringBuilder();
        foreach (var symbol in bufferedSymbols.OfType<IPaddingSymbol>())
        {
            if (symbol.Column is not { } column)
            {
                continue;
            }

            column.ObserveValueWidth(symbol.EstimateWidth(CurrentOptions.Culture, estimateBuffer));
        }

        foreach (var symbol in bufferedSymbols)
        {
            Write(symbol);
        }
    }
}