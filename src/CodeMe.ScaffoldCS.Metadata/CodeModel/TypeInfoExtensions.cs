namespace CodeMe.ScaffoldCS.Metadata.CodeModel;

public static class TypeInfoExtensions
{
    extension(TypeInfo type)
    {
        public TypeInfo ToNullable() => type with { IsNullable = true };

        public TypeInfo ToNotNullable() => type with { IsNullable = false };
    }
}