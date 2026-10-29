using System.Text;

namespace CodeMe.ScaffoldCS.Templates.Internals;

public interface IPaddingSymbol : ITemplateValueSymbol
{
    PaddingColumn? Column { get; set; }

    int EstimateWidth(IFormatProvider? formatProvider, StringBuilder? buffer = null);
}