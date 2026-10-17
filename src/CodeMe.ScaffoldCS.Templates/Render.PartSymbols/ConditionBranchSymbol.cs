using CodeMe.ScaffoldCS.Templates.Internals;
using CodeMe.ScaffoldCS.Templates.Output;

namespace CodeMe.ScaffoldCS.Templates.Render.PartSymbols;

public readonly struct ConditionBranchSymbol(
    ConditionBranch branch,
    bool condition,
    CurrentLineHandlingMode? lineHandling) : ITemplateControlSymbol
{
    public void Render(ITemplatePartWriter writer)
    {
        // We need to set line handling before and after branch switching
        // as we may toggle output on or off
        if (lineHandling != null)
        {
            writer.CurrentLineHandling = lineHandling.Value;
        }

        writer.BeginConditionBranch(branch, condition);

        if (lineHandling != null)
        {
            writer.CurrentLineHandling = lineHandling.Value;
        }
    }
}