namespace CodeMe.ScaffoldCS.Metadata.CodeModel;

public readonly record struct TypeName(
    string Name,
    string? Namespace = null);

public record TypeInfo(
    TypeName TypeName,
    TypeRole Role,
    bool IsNullable,
    IReadOnlyList<TypeInfo> GenericArgs)
{
    public string Name => TypeName.Name;

    public string? Namespace => TypeName.Namespace;
}

public enum TypeRole
{
    Void,

    Primitive,

    Collection,

    Dictionary,

    Dto
}

public record DtoModel(
    string Name,
    TypeName Type,
    string? Comment,
    IReadOnlyList<DtoField> Fields);

public record DtoField(string Name, TypeInfo Type, string? Comment);

public record ServiceModel(string Name, TypeInfo Type, string? Comment, IReadOnlyCollection<ServiceMethod> Methods);

public record ServiceMethod(string Name, TypeInfo ResultType, string? Comment, IReadOnlyList<MethodArg> Args);

public record MethodArg(string Name, TypeInfo Type, string? Comment);