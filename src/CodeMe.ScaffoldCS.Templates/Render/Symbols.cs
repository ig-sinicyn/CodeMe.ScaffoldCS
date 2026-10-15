using System.Runtime.CompilerServices;
using CodeMe.ScaffoldCS.Templates.Internals;
using CodeMe.ScaffoldCS.Templates.Output;
using CodeMe.ScaffoldCS.Templates.Redner.Symbols;
using CodeMe.ScaffoldCS.Templates.Render.Parts;

namespace CodeMe.ScaffoldCS.Templates.Render;

public static class Symbols
{
    private static readonly CurrentLineHandlingMode _macroLineHandling = CurrentLineHandlingMode.IgnoreIfWhiteSpace;

    public static readonly NewLineSymbol NewLine = new();

    public static readonly ClearLineSymbol ClearLine = new();

    public static readonly LineHandlingSymbol RemoveLine = new(CurrentLineHandlingMode.Ignore);

    public static readonly LineHandlingSymbol RemoveEmptyLine = new(CurrentLineHandlingMode.IgnoreIfEmpty);

    public static readonly LineHandlingSymbol RemoveWhitespaceLine = new(CurrentLineHandlingMode.IgnoreIfWhiteSpace);

    public static LineHandlingSymbol Comment(string comment) => new(_macroLineHandling);

    public static ConditionBranchSymbol If<T>(T condition) => new(
        (ConditionBranch)ConditionBranch.IfBranch, Conditions.HasValue(condition), (CurrentLineHandlingMode?)_macroLineHandling);

    public static ConditionBranchSymbol ElseIf<T>(T condition) => new(
        (ConditionBranch)ConditionBranch.ElseIfBranch, Conditions.HasValue(condition), (CurrentLineHandlingMode?)_macroLineHandling);

    public static readonly ConditionBranchSymbol Else = new((ConditionBranch)ConditionBranch.ElseBranch, (bool)false, (CurrentLineHandlingMode?)_macroLineHandling);

    public static readonly ConditionBranchSymbol EndIf = new((ConditionBranch)ConditionBranch.None, (bool)false, (CurrentLineHandlingMode?)_macroLineHandling);

    public static ConditionSymbol<TTrue> Iif<T, TTrue>(T condition, TTrue trueValue) =>
        new(Conditions.HasValue(condition), trueValue);

    public static ConditionSymbol<IfTrueTemplateString<T>> Iif<T>(
        T condition,
        [InterpolatedStringHandlerArgument(nameof(condition))] IfTrueTemplateString<T> trueValue) =>
        new(Conditions.HasValue(condition), trueValue);

    public static ConditionSymbol<TTrue, TFalse> Iif<T, TTrue, TFalse>(
        T condition,
        TTrue trueValue,
        TFalse falseValue) =>
        new(Conditions.HasValue(condition), trueValue, falseValue);

    public static ConditionSymbol<IfTrueTemplateString<T>, IfFalseTemplateString<T>> Iif<T>(
        T condition,
        [InterpolatedStringHandlerArgument(nameof(condition))] IfTrueTemplateString<T> trueValue,
        [InterpolatedStringHandlerArgument(nameof(condition))] IfFalseTemplateString<T> falseValue) =>
        new(Conditions.HasValue(condition), trueValue, falseValue);

    public static ConditionSymbol<IfTrueTemplateString<T>, TFalse> Iif<T, TFalse>(
        T condition,
        [InterpolatedStringHandlerArgument(nameof(condition))] IfTrueTemplateString<T> trueValue,
        TFalse falseValue) =>
        new(Conditions.HasValue(condition), trueValue, falseValue);

    public static ConditionSymbol<TTrue, IfFalseTemplateString<T>> Iif<T, TTrue>(
        T condition,
        TTrue trueValue,
        [InterpolatedStringHandlerArgument(nameof(condition))] IfFalseTemplateString<T> falseValue) =>
        new(Conditions.HasValue(condition), trueValue, falseValue);
}