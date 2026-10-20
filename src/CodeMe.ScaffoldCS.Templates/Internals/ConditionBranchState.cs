namespace CodeMe.ScaffoldCS.Templates.Internals;

public class ConditionBranchState
{
    private readonly Stack<BranchState> _branchStack = new();

    public bool ShouldWrite { get; private set; }

    public ConditionBranch CurrentBranch => Current.Branch;

    private BranchState Current => _branchStack.TryPeek(out var x) ? x : BranchState.NoBranch;

    public void BeginBranch(ConditionBranch branch, bool condition)
    {
        switch (branch)
        {
            case ConditionBranch.None:
                CompleteIfBranch();
                break;
            case ConditionBranch.IfBranch:
                BeginIfBranch(condition);
                break;
            case ConditionBranch.ElseIfBranch:
                BeginElseIfBranch(condition);
                break;
            case ConditionBranch.ElseBranch:
                BeginElseBranch();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(branch), branch, null);
        }
    }

    public void BeginIfBranch(bool condition)
    {
        var parent = Current;

        var shouldWrite = condition && parent.ConditionMet;
        _branchStack.Push(new BranchState(ConditionBranch.IfBranch, shouldWrite, AnyBranchMet: shouldWrite));
        ShouldWrite = shouldWrite;
    }

    public void BeginElseIfBranch(bool condition)
    {
        var previous = Current;
        if (previous.Branch is not (ConditionBranch.IfBranch or ConditionBranch.ElseIfBranch))
        {
            throw new InvalidOperationException($"Cannot begin ElseIf branch after {previous.Branch}");
        }

        var shouldWrite = condition && !previous.AnyBranchMet;
        var anyBranchMet = condition || previous.AnyBranchMet;
        _branchStack.Pop();
        _branchStack.Push(new BranchState(ConditionBranch.ElseIfBranch, shouldWrite, anyBranchMet));
        ShouldWrite = shouldWrite;
    }

    public void BeginElseBranch()
    {
        var previous = Current;
        if (previous.Branch is not (ConditionBranch.IfBranch or ConditionBranch.ElseIfBranch))
        {
            throw new InvalidOperationException($"Cannot begin Else branch after {previous.Branch}");
        }

        var shouldWrite = !previous.AnyBranchMet;
        var anyBranchMet = true;
        _branchStack.Pop();
        _branchStack.Push(new BranchState(ConditionBranch.ElseIfBranch, shouldWrite, anyBranchMet));
        ShouldWrite = shouldWrite;
    }

    public void CompleteIfBranch()
    {
        var previous = Current;
        if (previous.Branch == ConditionBranch.None)
        {
            throw new InvalidOperationException("Cannot complete if/else branch as there is no active branch");
        }

        _branchStack.Pop();
        var current = Current;
        ShouldWrite = current.ConditionMet;
    }

    private readonly record struct BranchState(ConditionBranch Branch, bool ConditionMet, bool AnyBranchMet)
    {
        public static readonly BranchState NoBranch = new(
            ConditionBranch.None, ConditionMet: true, AnyBranchMet: true);
    }
}