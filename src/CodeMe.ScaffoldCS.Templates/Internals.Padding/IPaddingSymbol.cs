using System.Text;

namespace CodeMe.ScaffoldCS.Templates.Internals.Padding;

public interface IPaddingSymbol : ITemplatePartSymbol
{
    IPaddingColumn? Column { get; set; }

    int EstimateWidth(IFormatProvider? formatProvider, StringBuilder? buffer = null);
}