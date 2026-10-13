using System.Runtime.CompilerServices;
using CodeMe.ScaffoldCS.Templates.Internals;

namespace CodeMe.ScaffoldCS.Templates.Render;

[InterpolatedStringHandler]
public readonly struct PaddedTemplatePart //: ITemplatePart
{
    private readonly ITemplate _template;

    public PaddedTemplatePart(int literalLength, int formattedCount)
        : this(literalLength, formattedCount, AmbientTemplate.Current)
    {
    }

    public PaddedTemplatePart(int literalLength, int formattedCount, ITemplate template)
    {
        _template = template;
    }
}