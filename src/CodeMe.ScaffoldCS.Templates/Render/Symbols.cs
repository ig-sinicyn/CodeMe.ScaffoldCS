using System.Runtime.CompilerServices;
using CodeMe.ScaffoldCS.Templates.Internals;
using CodeMe.ScaffoldCS.Templates.Output;

namespace CodeMe.ScaffoldCS.Templates.Render;

public static class Symbols
{
    private static readonly CurrentLineHandlingMode _macroLineHandling = CurrentLineHandlingMode.IgnoreIfWhiteSpace;

    public static readonly ValueSymbol<string> NewLine = new(Environment.NewLine);

    public static readonly ClearLineSymbol ClearLine = new();

    public static readonly LineHandlingSymbol RemoveLine = new(CurrentLineHandlingMode.Ignore);

    public static readonly LineHandlingSymbol RemoveEmptyLine = new(CurrentLineHandlingMode.IgnoreIfEmpty);

    public static readonly LineHandlingSymbol RemoveWhitespaceLine = new(CurrentLineHandlingMode.IgnoreIfWhiteSpace);

    public static ConditionBranchSymbol If<T>(T condition) => new(
        ConditionBranch.IfBranch, Conditions.HasValue(condition), _macroLineHandling);

    public static ConditionBranchSymbol ElseIf<T>(T condition) => new(
        ConditionBranch.ElseIfBranch, Conditions.HasValue(condition), _macroLineHandling);

    public static readonly ConditionBranchSymbol Else = new(ConditionBranch.ElseBranch, false, _macroLineHandling);

    public static readonly ConditionBranchSymbol EndIf = new(ConditionBranch.None, false, _macroLineHandling);

    public static ConditionSymbol<TTrue> Iif<T, TTrue>(T condition, TTrue trueValue) =>
        new(Conditions.HasValue(condition), trueValue);

    public static ConditionSymbol<IfTrueTemplatePart<T>> Iif<T>(
        T condition,
        [InterpolatedStringHandlerArgument(nameof(condition))] IfTrueTemplatePart<T> trueValue) =>
        new(Conditions.HasValue(condition), trueValue);

    public static ConditionSymbol<TTrue, TFalse> Iif<T, TTrue, TFalse>(
        T condition,
        TTrue trueValue,
        TFalse falseValue) =>
        new(Conditions.HasValue(condition), trueValue, falseValue);

    public static ConditionSymbol<IfTrueTemplatePart<T>, IfFalseTemplatePart<T>> Iif<T>(
        T condition,
        [InterpolatedStringHandlerArgument(nameof(condition))] IfTrueTemplatePart<T> trueValue,
        [InterpolatedStringHandlerArgument(nameof(condition))] IfFalseTemplatePart<T> falseValue) =>
        new(Conditions.HasValue(condition), trueValue, falseValue);

    public static ConditionSymbol<IfTrueTemplatePart<T>, TFalse> Iif<T, TFalse>(
        T condition,
        [InterpolatedStringHandlerArgument(nameof(condition))] IfTrueTemplatePart<T> trueValue,
        TFalse falseValue) =>
        new(Conditions.HasValue(condition), trueValue, falseValue);

    public static ConditionSymbol<TTrue, IfFalseTemplatePart<T>> Iif<T, TTrue>(
        T condition,
        TTrue trueValue,
        [InterpolatedStringHandlerArgument(nameof(condition))] IfFalseTemplatePart<T> falseValue) =>
        new(Conditions.HasValue(condition), trueValue, falseValue);
}