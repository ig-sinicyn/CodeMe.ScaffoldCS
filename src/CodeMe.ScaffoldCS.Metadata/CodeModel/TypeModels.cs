namespace CodeMe.ScaffoldCS.Metadata.CodeModel;

public readonly record struct TypeName(
    string Name,
    string? Namespace = null)
{
    public override string ToString() => Namespace == null ? Name : $"{Namespace}.{Name}";
}

public record TypeInfo(
    string Name,
    string? Namespace,
    TypeRole Role,
    bool IsNullable,
    IReadOnlyList<TypeInfo> GenericArgs)
{
    public static TypeInfo Primitive(string name) =>
        new(
            name,
            Namespace: null,
            Role: TypeRole.Primitive,
            IsNullable: false,
            []);

    internal static TypeInfo SystemPrimitive(string name, string? namespaceName = null) =>
        new(
            name,
            namespaceName ?? "System",
            Role: TypeRole.Primitive,
            IsNullable: false,
            []);

    public static TypeInfo Class(string name, string? namespaceName) =>
        new(
            name,
            namespaceName,
            Role: TypeRole.Dto,
            IsNullable: false,
            []);

    public TypeName TypeName => new(Name, Namespace);

    public override string ToString() => TypeName.ToString();
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
    string Name,
    string? Namespace,
    TypeRole Role,
    string? Comment,
    IReadOnlyList<DtoField> Fields)
{
    public DtoModel(string name, string? namespaceName, params DtoField[] fields)
        : this(name, namespaceName, TypeRole.Dto, null, fields)
    {
    }

    public DtoModel(string name, string? namespaceName, string? comment, params DtoField[] fields)
        : this(name, namespaceName, TypeRole.Dto, comment, fields)
    {
    }

    public TypeName TypeName => new(Name, Namespace);

    public override string ToString() => TypeName.ToString();
}

public record DtoField(string Name, TypeInfo Type, string? Comment = null)
{
    public object? DefaultValue { get; init; }

    public override string ToString() => $"{Type} {Name}";
}

public record DtoFieldInitializer(string Initializer);

public record ServiceModel(
    string Name,
    string? Namespace,
    string? Comment,
    IReadOnlyCollection<ServiceMethod> Methods)
{
    public ServiceModel(string name, string? namespaceName, params ServiceMethod[] methods)
        : this(name, namespaceName, null, methods)
    {
    }

    public TypeName TypeName => new(Name, Namespace);

    public override string ToString() => TypeName.ToString();
}

public record ServiceMethod(string Name, TypeInfo ResultType, string? Comment, IReadOnlyList<MethodArg> Args)
{
    public ServiceMethod(string name, TypeInfo resultType, params MethodArg[] args)
        : this(name, resultType, null, args)
    {
    }

    public override string ToString() => $"{ResultType} {Name} ({string.Join(',', Args)})";
}

public record MethodArg(string Name, TypeInfo Type, string? Comment = null)
{
    public object? DefaultValue { get; init; }

    public override string ToString() => $"{Type} {Name}";
}