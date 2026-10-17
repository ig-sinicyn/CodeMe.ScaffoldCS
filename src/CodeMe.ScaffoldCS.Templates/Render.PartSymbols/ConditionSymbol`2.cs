using CodeMe.ScaffoldCS.Templates.Internals;

namespace CodeMe.ScaffoldCS.Templates.Render.PartSymbols;

public readonly struct ConditionSymbol<TTrue, TFalse>(bool condition, TTrue trueValue, TFalse falseValue)
    : ITemplatePartSymbol
{
    public void Render(ITemplatePartWriter writer)
    {
        if (condition)
        {
            writer.Write(trueValue);
        }
        else
        {
            writer.Write(falseValue);
        }
    }
}