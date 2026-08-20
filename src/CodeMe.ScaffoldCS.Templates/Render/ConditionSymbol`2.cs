using CodeMe.ScaffoldCS.Templates.Internals;

namespace CodeMe.ScaffoldCS.Templates.Render;

public readonly struct ConditionSymbol<TTrue, TFalse>(
    bool condition,
    TTrue trueValue,
    TFalse falseValue)
    : ITemplateSymbol
{
    public void Render(Template template)
    {
        if (condition)
        {
            template.Write(trueValue);
        }
        else
        {
            template.Write(falseValue);
        }
    }
}