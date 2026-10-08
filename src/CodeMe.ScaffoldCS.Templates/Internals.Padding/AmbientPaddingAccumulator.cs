using CodeMe.Basics.Threading;

namespace CodeMe.ScaffoldCS.Templates.Internals.Padding;

public static class AmbientPaddingAccumulator
{
    private static readonly ScopedAsyncLocal<PaddingAccumulator> _accumulatorStack = new();

    internal static IDisposable BeginAccumulatorScope(PaddingAccumulator context) =>
        _accumulatorStack.BeginScope(context);

    public static Template Current => _templateStack.Current
        ?? throw new InvalidOperationException(
            $"There is no ambient template. Call {nameof(Template)}.{nameof(Template.BeginAmbientScope)} to enable ambient templates");

    public static Template? CurrentOrDefault => _templateStack.Current;
}