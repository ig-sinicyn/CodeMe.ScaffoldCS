namespace CodeMe.ScaffoldCS.Tool;

public class TemplateLoadException : Exception
{
    public TemplateLoadException()
    {
    }

    public TemplateLoadException(string message) : base(message)
    {
    }

    public TemplateLoadException(string message, Exception inner) : base(message, inner)
    {
    }
}