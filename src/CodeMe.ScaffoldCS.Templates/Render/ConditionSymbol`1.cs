using CodeMe.ScaffoldCS.Templates.Internals;

namespace CodeMe.ScaffoldCS.Templates.Render;

public readonly struct ConditionSymbol<TTrue>(
    bool condition,
    TTrue trueValue)
    : ITemplateSymbol
{
    public void Render(Template template)
    {
        if (condition)
        {
            template.Write(trueValue);
        }
    }
}