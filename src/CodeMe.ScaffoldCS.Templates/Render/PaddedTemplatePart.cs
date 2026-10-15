using System.Runtime.CompilerServices;
using CodeMe.ScaffoldCS.Templates.Internals;

namespace CodeMe.ScaffoldCS.Templates.Render;

[InterpolatedStringHandler]
public readonly struct PaddedTemplatePart //: ITemplatePart
{
    private readonly ITemplateWriter _writer;

    public PaddedTemplatePart(int literalLength, int formattedCount)
        : this(literalLength, formattedCount, AmbientTemplate.Current)
    {
    }

    public PaddedTemplatePart(int literalLength, int formattedCount, ITemplateWriter writer)
    {
        _writer = writer;
    }
}