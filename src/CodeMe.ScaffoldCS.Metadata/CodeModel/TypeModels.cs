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

    Enum,

    Collection,

    Dictionary,

    Dto
}

public record DtoModel(
    TypeName Type,
    TypeRole Role,
    string? Comment,
    IReadOnlyList<DtoField> Fields)
{
    public DtoModel(string name, string? namespaceName, string? comment, params DtoField[] fields)
        : this(new TypeName(name, namespaceName), TypeRole.Dto, comment, fields)
    {
    }

    public string Name => Type.Name;

    public string? Namespace => Type.Namespace;
}

public record DtoField(string Name, TypeInfo Type, string? Comment);

public record ServiceModel(string Name, TypeInfo Type, string? Comment, IReadOnlyCollection<ServiceMethod> Methods);

public record ServiceMethod(string Name, TypeInfo ResultType, string? Comment, IReadOnlyList<MethodArg> Args);

public record MethodArg(string Name, TypeInfo Type, string? Comment);