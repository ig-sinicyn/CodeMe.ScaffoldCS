namespace CodeMe.ScaffoldCS.Metadata.CodeModel;

public readonly record struct TypeName(
    string Name,
    string? Namespace);

public record TypeInfo(
    TypeName TypeName,
    TypeKind Kind,
    bool IsNullable,
    IReadOnlyList<TypeInfo> GenericArgs)
{
    public string Name => TypeName.Name;

    public string? Namespace => TypeName.Namespace;
}

public enum TypeKind
{
    Void,

    Primitive,

    Entity,

    Collection,

    Dictionary
}

public record Entity(
    string Name,
    TypeName Type,
    string? Comment,
    IReadOnlyList<EntityField> Fields);

public record EntityField(string Name, TypeInfo Type, string? Comment);

public record EntityService(string Name, TypeInfo Type, string? Comment, IReadOnlyCollection<EntityMethod> Methods);

public record EntityMethod(string Name, TypeInfo ResultType, string? Comment, IReadOnlyList<EntityArg> Args);

public record EntityArg(string Name, TypeInfo Type, string? Comment);