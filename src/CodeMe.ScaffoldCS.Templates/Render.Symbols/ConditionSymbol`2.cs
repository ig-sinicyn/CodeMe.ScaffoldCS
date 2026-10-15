using CodeMe.ScaffoldCS.Templates.Internals;

namespace CodeMe.ScaffoldCS.Templates.Render.Parts;

public readonly struct ConditionSymbol<TTrue, TFalse>(bool condition, TTrue trueValue, TFalse falseValue)
    : ITemplateSymbol
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