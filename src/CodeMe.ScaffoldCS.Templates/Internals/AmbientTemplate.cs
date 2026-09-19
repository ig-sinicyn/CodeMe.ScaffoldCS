using CodeMe.Basics.Threading;

namespace CodeMe.ScaffoldCS.Templates.Internals;

public static class AmbientTemplate
{
    private static readonly ScopedAsyncLocal<Template> _templateStack = new();

    internal static IDisposable BeginTemplateScope(Template context) => _templateStack.BeginScope(context);

    public static Template Current => _templateStack.Current
        ?? throw new InvalidOperationException(
            $"There is no ambient template. Call {nameof(Template)}.{nameof(Template.BeginAmbientScope)} to enable ambient templates");

    public static Template? CurrentOrDefault => _templateStack.Current;
}