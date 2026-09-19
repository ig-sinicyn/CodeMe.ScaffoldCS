using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace CodeMe.ScaffoldCS.Templates.Render;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class SymbolsCS
{
    public static readonly ValueSymbol<string> NL = Symbols.NewLine;

    public static readonly ClearLineSymbol CL = Symbols.ClearLine;

    public static readonly LineHandlingSymbol RL = Symbols.RemoveLine;

    public static readonly LineHandlingSymbol REL = Symbols.RemoveEmptyLine;

    public static readonly LineHandlingSymbol RWL = Symbols.RemoveWhitespaceLine;

    public static ConditionBranchSymbol IF<T>(T value) => Symbols.If(value);

    public static ConditionBranchSymbol ELSEIF<T>(T value) => Symbols.ElseIf(value);

    public static readonly ConditionBranchSymbol ELSE = Symbols.Else;

    public static readonly ConditionBranchSymbol ENDIF = Symbols.EndIf;

    public static ConditionSymbol<TTrue> IIF<T, TTrue>(T condition, TTrue trueValue) =>
        Symbols.Iif(condition, trueValue);

    public static ConditionSymbol<IfTrueTemplatePart<T>> IIF<T>(
        T condition,
        [InterpolatedStringHandlerArgument(nameof(condition))] IfTrueTemplatePart<T> trueValue) =>
        Symbols.Iif(condition, trueValue);

    public static ConditionSymbol<TTrue, TFalse> IIF<T, TTrue, TFalse>(T condition, TTrue trueValue, TFalse falseValue) =>
        Symbols.Iif(condition, trueValue, falseValue);

    public static ConditionSymbol<IfTrueTemplatePart<T>, IfFalseTemplatePart<T>> IIF<T>(
        T condition,
        [InterpolatedStringHandlerArgument(nameof(condition))] IfTrueTemplatePart<T> trueValue,
        [InterpolatedStringHandlerArgument(nameof(condition))] IfFalseTemplatePart<T> falseValue) =>
        Symbols.Iif(condition, trueValue, falseValue);

    public static ConditionSymbol<IfTrueTemplatePart<T>, TFalse> IIF<T, TFalse>(
        T condition,
        [InterpolatedStringHandlerArgument(nameof(condition))] IfTrueTemplatePart<T> trueValue,
        TFalse falseValue) =>
        Symbols.Iif(condition, trueValue, falseValue);

    public static ConditionSymbol<TTrue, IfFalseTemplatePart<T>> IIF<T, TTrue>(
        T condition,
        TTrue trueValue,
        [InterpolatedStringHandlerArgument(nameof(condition))] IfFalseTemplatePart<T> falseValue) =>
        Symbols.Iif(condition, trueValue, falseValue);
}