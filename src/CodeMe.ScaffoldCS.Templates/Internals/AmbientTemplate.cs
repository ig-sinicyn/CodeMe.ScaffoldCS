using CodeMe.Basics.Threading;

namespace CodeMe.ScaffoldCS.Templates.Internals;

public static class AmbientTemplate
{
    private static readonly ScopedAsyncLocal<ITemplatePartWriter> _templateStack = new();

    internal static IDisposable BeginTemplateScope(ITemplatePartWriter writer) => _templateStack.BeginScope(writer);

    public static ITemplateWriter Current => _templateStack.Current
        ?? throw new InvalidOperationException(
            $"There is no ambient template. Call {nameof(Template)}.{nameof(Template.BeginAmbientScope)} to enable ambient templates");

    public static ITemplateWriter? CurrentOrDefault => _templateStack.Current;
}