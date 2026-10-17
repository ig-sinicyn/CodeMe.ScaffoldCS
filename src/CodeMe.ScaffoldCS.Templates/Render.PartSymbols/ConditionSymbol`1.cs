using CodeMe.ScaffoldCS.Templates.Internals;

namespace CodeMe.ScaffoldCS.Templates.Render.PartSymbols;

public readonly struct ConditionSymbol<TTrue>(bool condition, TTrue trueValue) : ITemplatePartSymbol
{
    public void Render(ITemplatePartWriter writer)
    {
        if (condition)
        {
            writer.Write(trueValue);
        }
    }
}