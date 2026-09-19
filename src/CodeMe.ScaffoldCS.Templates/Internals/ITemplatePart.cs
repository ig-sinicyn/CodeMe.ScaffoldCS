namespace CodeMe.ScaffoldCS.Templates.Internals;

public interface ITemplatePart : ITemplateScope
{
    void AppendLiteral(string? value);

    void AppendFormatted<T>(T? value);

    void AppendFormatted<T>(T? value, int alignment);

    void AppendFormatted<T>(T? value, int alignment, string? format);

    void AppendFormatted<T>(T? value, string? format);

    void AppendFormatted(string? value);

    void AppendFormatted(string? value, int alignment);

    void AppendFormatted(Action<Template> callback);

    void AppendFormatted(Func<TemplatePart> callback);

    void AppendFormatted<T>(IEnumerable<T> values);
}