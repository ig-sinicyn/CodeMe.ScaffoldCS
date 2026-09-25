using static CodeMe.ScaffoldCS.Metadata.CodeModel.TypeInfo;

namespace CodeMe.ScaffoldCS.Metadata.CodeModel;

public static class WellKnownTypes
{
    public static readonly TypeInfo Boolean = Primitive("bool");

    public static readonly TypeInfo Byte = Primitive("byte");

    public static readonly TypeInfo SByte = Primitive("sbyte");

    public static readonly TypeInfo Char = Primitive("char");

    public static readonly TypeInfo Decimal = Primitive("decimal");

    public static readonly TypeInfo Double = Primitive("double");

    public static readonly TypeInfo Single = Primitive("float");

    public static readonly TypeInfo Int32 = Primitive("int");

    public static readonly TypeInfo UInt32 = Primitive("uint");

    public static readonly TypeInfo IntPtr = Primitive("nint");

    public static readonly TypeInfo UIntPtr = Primitive("nuint");

    public static readonly TypeInfo Int64 = Primitive("long");

    public static readonly TypeInfo UInt64 = Primitive("ulong");

    public static readonly TypeInfo Int16 = Primitive("short");

    public static readonly TypeInfo UInt16 = Primitive("ushort");

    public static readonly TypeInfo String = Primitive("string");

    public static readonly TypeInfo DateTime = Primitive("DateTime");

    public static readonly TypeInfo DateOnly = Primitive("DateOnly");

    public static readonly TypeInfo DateTimeOffset = Primitive("DateTimeOffset");

    public static readonly TypeInfo TimeSpan = Primitive("TimeSpan");

    public static readonly TypeInfo TimeOnly = Primitive("TimeOnly");

    public static readonly TypeInfo Guid = Primitive("Guid");

    public static readonly TypeInfo Index = Primitive("Index");

    public static readonly TypeInfo Range = Primitive("Range");

    public static readonly TypeInfo Half = Primitive("Half");

    public static readonly TypeInfo BigInteger = SystemPrimitive("BigInteger", "System.Numerics");

    public static readonly TypeInfo Complex = SystemPrimitive("Complex", "System.Numerics");
}