using CodeMe.ScaffoldCS.Templates.Internals;
using CodeMe.ScaffoldCS.Templates.Output;

namespace CodeMe.ScaffoldCS.Templates;

public partial class Template
{
    private readonly Stack<TemplateTextWriterOptions> _optionsStack = new();
    private readonly Stack<BranchState> _branchesStack = new();

    private BranchState CurrentBranch => _branchesStack.TryPeek(out var x) ? x : BranchState.NoBranch;

    public OptionsScope BeginOptionsScope(IndentationFormat indentationFormat) =>
        BeginOptionsScope(NewOptions(indentationFormat));

    public OptionsScope BeginOptionsScope(TemplateTextWriterOptions options)
    {
        var oldOptions = Options;
        _optionsStack.Push(oldOptions);
        Options = options;
        return new OptionsScope(this, oldOptions);
    }

    internal OptionsScope BeginTemplatePartScope() =>
        BeginOptionsScope(IndentationFormat.CurrentLine);

    private TemplateTextWriterOptions NewOptions(IndentationFormat indentationFormat) => Options with
    {
        Indentation = _writer.GetCurrentLineIndentation(indentationFormat)
    };

    private void CompleteScope(TemplateTextWriterOptions options)
    {
        if (_optionsStack.Count == 0 || !ReferenceEquals(_optionsStack.Peek(), options))
        {
            throw new InvalidOperationException("Cannot complete scope as current options do not match expected ones.");
        }

        Options = options;
        _optionsStack.Pop();
    }

    public void BeginIfBranch(bool condition)
    {
        var parent = CurrentBranch;

        var shouldWrite = condition && parent.ConditionMet;
        _branchesStack.Push(new BranchState(ConditionBranch.IfBranch, shouldWrite, shouldWrite));
        ShouldWrite = shouldWrite;
    }

    public void BeginElseIfBranch(bool condition)
    {
        var previous = CurrentBranch;
        if (previous.Branch is not (ConditionBranch.IfBranch or ConditionBranch.ElseIfBranch))
        {
            throw new InvalidOperationException($"Cannot begin ElseIf branch after {previous.Branch}");
        }

        var shouldWrite = condition && !previous.AnyBranchMet;
        var anyBranchMet = condition || previous.AnyBranchMet;
        _branchesStack.Pop();
        _branchesStack.Push(new BranchState(ConditionBranch.ElseIfBranch, shouldWrite, anyBranchMet));
        ShouldWrite = shouldWrite;
    }

    public void BeginElseBranch()
    {
        var previous = CurrentBranch;
        if (previous.Branch is not (ConditionBranch.IfBranch or ConditionBranch.ElseIfBranch))
        {
            throw new InvalidOperationException($"Cannot begin Else branch after {previous.Branch}");
        }

        var shouldWrite = !previous.AnyBranchMet;
        var anyBranchMet = true;
        _branchesStack.Pop();
        _branchesStack.Push(new BranchState(ConditionBranch.ElseIfBranch, shouldWrite, anyBranchMet));
        ShouldWrite = shouldWrite;
    }

    public void CompleteIfBranch()
    {
        var previous = CurrentBranch;
        if (previous.Branch == ConditionBranch.None)
        {
            throw new InvalidOperationException("Cannot complete if/else branch as there is no active branch");
        }

        _branchesStack.Pop();
        var current = CurrentBranch;
        ShouldWrite = current.ConditionMet;
    }

    public readonly struct OptionsScope(Template owner, TemplateTextWriterOptions oldOptions)
        : ITemplateScope, IDisposable
    {
        public void DisposeIfSame(Template caller)
        {
            if (ReferenceEquals(caller, owner))
            {
                Dispose();
            }
        }

        // ReSharper disable once ConditionalAccessQualifierIsNonNullableAccordingToAPIContract
        public void Dispose() => owner?.CompleteScope(oldOptions);
    }

    internal readonly record struct BranchState(ConditionBranch Branch, bool ConditionMet, bool AnyBranchMet)
    {
        public static readonly BranchState NoBranch = new(
            ConditionBranch.None, ConditionMet: true, AnyBranchMet: true);
    }
}