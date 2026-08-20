using CodeMe.ScaffoldCS.Templates.Internals;

namespace CodeMe.ScaffoldCS.Templates.Render;

public record CollectionRenderOptions(
    string? Separator = null,
    ITemplateSymbol? EmptyValue = null,
    int Alignment = 0,
    string? Format = null)
{
    public static readonly CollectionRenderOptions CommaSeparated = new(Separator: ", ");

    public static readonly CollectionRenderOptions Multiline = new(Separator: Environment.NewLine);

    public static readonly CollectionRenderOptions EmptyLineSeparated =
        new(
            Separator: Environment.NewLine + Environment.NewLine,
            EmptyValue: Symbols.RemoveWhitespaceLine);

    public static readonly CollectionRenderOptions Default = Multiline;
}