using System.Runtime.CompilerServices;
using CodeMe.ScaffoldCS.Templates.Internals;

namespace CodeMe.ScaffoldCS.Templates.Render;

[InterpolatedStringHandler]
public readonly struct PaddedTemplatePart //: ITemplatePart
{
    private readonly ITemplateValueWriter _writer;

    public PaddedTemplatePart(int literalLength, int formattedCount)
        : this(literalLength, formattedCount, AmbientTemplate.Current)
    {
    }

    public PaddedTemplatePart(int literalLength, int formattedCount, ITemplateValueWriter writer)
    {
        _writer = writer;
    }
}