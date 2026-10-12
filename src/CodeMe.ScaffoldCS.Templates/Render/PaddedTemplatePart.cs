using System.Runtime.CompilerServices;
using CodeMe.ScaffoldCS.Templates.Internals;

namespace CodeMe.ScaffoldCS.Templates.Render;

[InterpolatedStringHandler]
public readonly struct PaddedTemplatePart //: ITemplatePart
{
    private readonly Template _template;

    public PaddedTemplatePart(int literalLength, int formattedCount)
        : this(literalLength, formattedCount, AmbientTemplate.Current)
    {
    }

    public PaddedTemplatePart(int literalLength, int formattedCount, Template template)
    {
        _template = template;
    }
}