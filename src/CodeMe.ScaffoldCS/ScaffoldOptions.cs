namespace CodeMe.ScaffoldCS;

public class ScaffoldOptions
{
    public string? OutputPath { get; set; }

    public string? ModelPath { get; set; }

    public string? ModelBasePath { get; set; }

    public bool AllowOverwrite { get; set; }

    public string? EncodingName { get; set; }

    public ScaffoldRenderOptions Render { get; } = new();
}