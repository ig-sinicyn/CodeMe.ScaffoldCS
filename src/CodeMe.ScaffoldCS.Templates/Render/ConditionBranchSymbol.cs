using CodeMe.ScaffoldCS.Templates.Internals;
using CodeMe.ScaffoldCS.Templates.Output;

namespace CodeMe.ScaffoldCS.Templates.Render;

public readonly struct ConditionBranchSymbol(
    ConditionBranch branch,
    bool condition,
    CurrentLineHandlingMode? lineHandling)
    : IControlTemplateSymbol
{
    public void Render(Template template)
    {
        // We need to set line handling before and after branch switching
        // as we may toggle output on or off
        if (lineHandling != null)
        {
            template.SetLineHandling(lineHandling.Value);
        }

        switch (branch)
        {
            case ConditionBranch.None:
                template.CompleteIfBranch();
                break;
            case ConditionBranch.IfBranch:
                template.BeginIfBranch(condition);
                break;
            case ConditionBranch.ElseIfBranch:
                template.BeginElseIfBranch(condition);
                break;
            case ConditionBranch.ElseBranch:
                template.BeginElseBranch();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(branch), branch, null);
        }

        if (lineHandling != null)
        {
            template.SetLineHandling(lineHandling.Value);
        }
    }
}