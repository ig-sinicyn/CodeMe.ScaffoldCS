using CodeMe.Basics.Threading;

namespace CodeMe.ScaffoldCS.Templates.Internals;

public static class AmbientTemplate
{
    private static readonly ScopedAsyncLocal<ITemplatePartWriter> _templateStack = new();

    internal static IDisposable BeginScope(ITemplatePartWriter writer) => _templateStack.BeginScope(writer);

    public static ITemplatePartWriter Current => _templateStack.Current
        ?? throw new InvalidOperationException(
            $"There is no ambient template. Call {nameof(Template)}.{nameof(Template.BeginAmbientScope)} to enable ambient templates");

    public static ITemplatePartWriter? CurrentOrDefault => _templateStack.Current;
}