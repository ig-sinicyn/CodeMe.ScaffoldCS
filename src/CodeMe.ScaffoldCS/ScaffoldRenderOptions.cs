using CodeMe.ScaffoldCS.Templates.Output;

namespace CodeMe.ScaffoldCS;

public class ScaffoldRenderOptions
{
    public string? CultureName { get; set; }

    public NewLineFormat NewLineFormat { get; set; } = NewLineFormat.Auto;
}