using System.Runtime.CompilerServices;
using CodeMe.ScaffoldCS.Templates.Internals;
using CodeMe.ScaffoldCS.Templates.Output;
using CodeMe.ScaffoldCS.Templates.Render.PartSymbols;
using CodeMe.ScaffoldCS.Templates.Render.ValueSymbols;

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
        ConditionBranch.IfBranch, Conditions.HasValue(condition), _macroLineHandling);

    public static ConditionBranchSymbol ElseIf<T>(T condition) => new(
        ConditionBranch.ElseIfBranch, Conditions.HasValue(condition), _macroLineHandling);

    public static readonly ConditionBranchSymbol Else = new(ConditionBranch.ElseBranch, false, _macroLineHandling);

    public static readonly ConditionBranchSymbol EndIf = new(ConditionBranch.None, false, _macroLineHandling);

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

    public static PaddingSymbol<T> PadLeft<T>(T value, int minLength = 0) => 
        new(value, PaddingRenderOptions.LeftPadding, minLength);

    public static PaddingSymbol<T> PadLeft<T>(T value, char paddingChar = ' ', int minLength = 0) =>
        new(value, new PaddingRenderOptions(PaddingMode.Left, paddingChar, minLength));

    public static PaddingSymbol<T> PadRight<T>(T value, int minLength = 0) =>
        new(value, PaddingRenderOptions.RightPadding, minLength);

    public static PaddingSymbol<T> PadRight<T>(T value, char paddingChar = ' ', int minLength = 0) =>
        new(value, new PaddingRenderOptions(PaddingMode.Right, paddingChar, minLength));

    public static PaddingSymbol<T> PadCenter<T>(T value, int minLength = 0) =>
        new(value, PaddingRenderOptions.CenterPadding, minLength);

    public static PaddingSymbol<T> PadCenter<T>(T value, char paddingChar = ' ', int minLength = 0) =>
        new(value, new PaddingRenderOptions(PaddingMode.Center, paddingChar, minLength));
}