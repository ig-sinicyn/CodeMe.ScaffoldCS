using CodeMe.Basics.Threading;

namespace CodeMe.ScaffoldCS.Templates.Internals;

public static class AmbientTemplate
{
    private static readonly ScopedAsyncLocal<ITemplate> _templateStack = new();

    internal static IDisposable BeginTemplateScope(ITemplate template) => _templateStack.BeginScope(template);

    public static ITemplate Current => _templateStack.Current
        ?? throw new InvalidOperationException(
            $"There is no ambient template. Call {nameof(Template)}.{nameof(Template.BeginAmbientScope)} to enable ambient templates");

    public static ITemplate? CurrentOrDefault => _templateStack.Current;
}