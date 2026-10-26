using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using CodeMe.ScaffoldCS.Templates.Render.PartSymbols;
using CodeMe.ScaffoldCS.Templates.Render.ValueSymbols;

namespace CodeMe.ScaffoldCS.Templates.Render;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class SymbolsCS
{
    public static readonly NewLineSymbol NL = Symbols.NewLine;

    public static readonly ClearLineSymbol CL = Symbols.ClearLine;

    public static readonly LineHandlingSymbol RL = Symbols.RemoveLine;

    public static readonly LineHandlingSymbol REL = Symbols.RemoveEmptyLine;

    public static readonly LineHandlingSymbol RWL = Symbols.RemoveWhitespaceLine;

    public static LineHandlingSymbol COMMENT(string comment) => Symbols.Comment(comment);

    public static ConditionBranchSymbol IF<T>(T value) => Symbols.If(value);

    public static ConditionBranchSymbol ELSEIF<T>(T value) => Symbols.ElseIf(value);

    public static readonly ConditionBranchSymbol ELSE = Symbols.Else;

    public static readonly ConditionBranchSymbol ENDIF = Symbols.EndIf;

    public static ConditionSymbol<TTrue> IIF<T, TTrue>(T condition, TTrue trueValue) =>
        Symbols.Iif(condition, trueValue);

    public static ConditionSymbol<IfTrueTemplateString<T>> IIF<T>(
        T condition,
        [InterpolatedStringHandlerArgument(nameof(condition))] IfTrueTemplateString<T> trueValue) =>
        Symbols.Iif(condition, trueValue);

    public static ConditionSymbol<TTrue, TFalse> IIF<T, TTrue, TFalse>(
        T condition,
        TTrue trueValue,
        TFalse falseValue) =>
        Symbols.Iif(condition, trueValue, falseValue);

    public static ConditionSymbol<IfTrueTemplateString<T>, IfFalseTemplateString<T>> IIF<T>(
        T condition,
        [InterpolatedStringHandlerArgument(nameof(condition))] IfTrueTemplateString<T> trueValue,
        [InterpolatedStringHandlerArgument(nameof(condition))] IfFalseTemplateString<T> falseValue) =>
        Symbols.Iif(condition, trueValue, falseValue);

    public static ConditionSymbol<IfTrueTemplateString<T>, TFalse> IIF<T, TFalse>(
        T condition,
        [InterpolatedStringHandlerArgument(nameof(condition))] IfTrueTemplateString<T> trueValue,
        TFalse falseValue) =>
        Symbols.Iif(condition, trueValue, falseValue);

    public static ConditionSymbol<TTrue, IfFalseTemplateString<T>> IIF<T, TTrue>(
        T condition,
        TTrue trueValue,
        [InterpolatedStringHandlerArgument(nameof(condition))] IfFalseTemplateString<T> falseValue) =>
        Symbols.Iif(condition, trueValue, falseValue);

    public static PaddingSymbol<T> LPAD<T>(T value, int minLength = 0) =>
        Symbols.PadLeft(value, minLength);

    public static PaddingSymbol<T> LPAD<T>(T value, char paddingChar = ' ', int minLength = 0) =>
        Symbols.PadLeft(value, paddingChar, minLength);

    public static PaddingSymbol<T> RPAD<T>(T value, int minLength = 0) =>
        Symbols.PadRight(value, minLength);

    public static PaddingSymbol<T> RPAD<T>(T value, char paddingChar = ' ', int minLength = 0) =>
        Symbols.PadRight(value, paddingChar, minLength);

    public static PaddingSymbol<T> CENTER<T>(T value, int minLength = 0) =>
        Symbols.PadCenter(value, minLength);

    public static PaddingSymbol<T> CENTER<T>(T value, char paddingChar = ' ', int minLength = 0) =>
        Symbols.PadCenter(value, paddingChar, minLength);
}