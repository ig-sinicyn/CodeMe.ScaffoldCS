using System.Text;
using CodeMe.ScaffoldCS.Templates.Internals;

namespace CodeMe.ScaffoldCS.Templates.Render.Padding;

public interface IPaddingSymbol : ITemplateValueSymbol
{
    IPaddingColumn? Column { get; set; }

    int EstimateWidth(IFormatProvider? formatProvider, StringBuilder? buffer = null);
}