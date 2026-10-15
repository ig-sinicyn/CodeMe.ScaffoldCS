using CodeMe.ScaffoldCS.Templates.Internals;

namespace CodeMe.ScaffoldCS.Templates.Render.Parts;

public readonly struct ConditionSymbol<TTrue>(bool condition, TTrue trueValue) : ITemplateSymbol
{
    public void Render(ITemplatePartWriter writer)
    {
        if (condition)
        {
            writer.Write(trueValue);
        }
    }
}