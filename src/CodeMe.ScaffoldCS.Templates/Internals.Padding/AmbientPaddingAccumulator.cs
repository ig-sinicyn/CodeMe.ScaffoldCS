using CodeMe.Basics.Threading;

namespace CodeMe.ScaffoldCS.Templates.Internals.Padding;

public static class AmbientPaddingAccumulator
{
    private static readonly ScopedAsyncLocal<PaddingAccumulator> _accumulatorStack = new();

    internal static IDisposable BeginAccumulatorScope(PaddingAccumulator accumulator) =>
        _accumulatorStack.BeginScope(accumulator);

    public static PaddingAccumulator Current => _accumulatorStack.Current
        ?? throw new InvalidOperationException(
            $"There is no ambient accumulator. Call {nameof(PaddingAccumulator)}.{nameof(PaddingAccumulator.BeginAmbientScope)} to enable ambient accumulator");

    public static PaddingAccumulator? CurrentOrDefault => _accumulatorStack.Current;
}