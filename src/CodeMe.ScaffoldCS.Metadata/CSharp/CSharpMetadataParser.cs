using CodeMe.ScaffoldCS.Metadata.CodeModel;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using TypeInfo = CodeMe.ScaffoldCS.Metadata.CodeModel.TypeInfo;

namespace CodeMe.ScaffoldCS.Metadata.CSharp;

internal static class CSharpMetadataParser
{
    public static BaseTypeDeclarationSyntax ResolveTargetType(CompilationUnitSyntax root, string typeName)
    {
        var candidates = root.DescendantNodes()
            .OfType<BaseTypeDeclarationSyntax>()
            .Where(
                type => type is RecordDeclarationSyntax
                    or ClassDeclarationSyntax
                    or InterfaceDeclarationSyntax
                    or RecordDeclarationSyntax
                    or StructDeclarationSyntax
                    or EnumDeclarationSyntax)
            .ToArray();

        if (candidates.Length == 0)
        {
            throw new InvalidOperationException($"No class or interface named '{typeName}' was found in the file.");
        }

        if (string.IsNullOrWhiteSpace(typeName))
        {
            return candidates[0];
        }

        return candidates.FirstOrDefault(type => type.NameMatches(typeName))
            ?? candidates.FirstOrDefault(type => type.FullNameMatches(typeName))
            ?? throw new InvalidOperationException($"No class or interface named '{typeName}' was found in the file.");
    }

    public static DtoModel ParseDto(BaseTypeDeclarationSyntax declaration, CSharpCompilation compilation) =>
        declaration switch
        {
            TypeDeclarationSyntax x => ParseDto(x, compilation),
            EnumDeclarationSyntax x => ParseDto(x, compilation),
            _ => throw new InvalidOperationException($"Unsupported declaration {declaration.GetType().Name}")
        };

    private static DtoModel ParseDto(TypeDeclarationSyntax declaration, CSharpCompilation compilation)
    {
        var model = compilation.GetSemanticModel(declaration.SyntaxTree);
        var symbol = model.GetDeclaredSymbol(declaration)
            ?? throw new InvalidOperationException(
                $"Unable to resolve the target type '{declaration.Name}'.");

        var fields = declaration.Members
            .OfType<PropertyDeclarationSyntax>()
            .Select(x => ParseDtoField(x, compilation))
            .ToArray();

        return new DtoModel(
            new TypeName(symbol.Name, symbol.Namespace),
            TypeRole.Dto,
            symbol.GetDocumentationSummary(),
            fields);
    }

    private static DtoModel ParseDto(EnumDeclarationSyntax declaration, CSharpCompilation compilation)
    {
        var model = compilation.GetSemanticModel(declaration.SyntaxTree);
        var symbol = model.GetDeclaredSymbol(declaration)
            ?? throw new InvalidOperationException(
                $"Unable to resolve the target type '{declaration.Name}'.");

        return new DtoModel(
            new TypeName(symbol.Name, symbol.Namespace),
            TypeRole.Enum,
            symbol.GetDocumentationSummary(),
            []);
    }

    public static DtoField ParseDtoField(
        PropertyDeclarationSyntax declaration,
        CSharpCompilation compilation)
    {
        var model = compilation.GetSemanticModel(declaration.SyntaxTree);
        var symbol = model.GetDeclaredSymbol(declaration)
            ?? throw new InvalidOperationException(
                $"Unable to resolve the target symbol '{declaration.Identifier}'.");

        return new DtoField(
            symbol.Name,
            Resolve(declaration.Type, model),
            symbol.GetDocumentationSummary());
    }

    public static DtoField ParseDtoField(
        EnumDeclarationSyntax declaration,
        CSharpCompilation compilation)
    {
        var model = compilation.GetSemanticModel(declaration.SyntaxTree);
        var symbol = model.GetDeclaredSymbol(declaration)
            ?? throw new InvalidOperationException(
                $"Unable to resolve the target symbol '{declaration.Identifier}'.");

        var underlyingEnumType =
            declaration.BaseList?.Types.FirstOrDefault()?.ToString()
            ?? "int";

        return new DtoField(
            symbol.Name,
            Resolve(null!, model),
            symbol.GetDocumentationSummary());
    }

    private static TypeInfo Resolve(TypeSyntax typeSyntax, SemanticModel semanticModel)
    {
        var typeSymbol = semanticModel.GetTypeInfo(typeSyntax).Type;
        if (typeSymbol is null)
        {
            return new TypeInfo(new TypeName(typeSyntax.ToString()), TypeRole.Primitive, false, []);
        }

        return Resolve(
            typeSymbol,
            typeSyntax is NullableTypeSyntax
            || typeSymbol.OriginalDefinition.SpecialType == SpecialType.System_Nullable_T);
    }

    private static TypeInfo Resolve(ITypeSymbol typeSymbol, bool isNullable)
    {
        // TODO: check for IsNullable
        if (typeSymbol is IArrayTypeSymbol arrayTypeSymbol)
        {
            return new TypeInfo(
                new TypeName(arrayTypeSymbol.ElementType.Name, arrayTypeSymbol.ElementType.Namespace),
                TypeRole.Collection,
                isNullable,
                new[] { Resolve(arrayTypeSymbol.ElementType, false) });
        }

        var typeName = new TypeName(typeSymbol.Name, typeSymbol.Namespace);

        if (typeSymbol is INamedTypeSymbol namedTypeSymbol)
        {
            if (namedTypeSymbol.IsGenericType)
            {
                if (IsDictionary(namedTypeSymbol))
                {
                    return new TypeInfo(
                        typeName,
                        TypeRole.Dictionary,
                        isNullable,
                        namedTypeSymbol.TypeArguments.Select(arg => Resolve(arg, false)).ToArray());
                }

                if (IsCollection(namedTypeSymbol))
                {
                    return new TypeInfo(
                        typeName,
                        TypeRole.Collection,
                        isNullable,
                        namedTypeSymbol.TypeArguments.Select(arg => Resolve(arg, false)).ToArray());
                }

                if (IsNullableStruct(namedTypeSymbol))
                {
                    return Resolve(namedTypeSymbol.TypeArguments[0], true);
                }
            }

            if (IsPrimitive(namedTypeSymbol))
            {
                return new TypeInfo(
                    new TypeName(SymbolDisplay.ToDisplayString(namedTypeSymbol)),
                    TypeRole.Primitive,
                    isNullable,
                    []);
            }

            return new TypeInfo(
                typeName,
                TypeRole.Dto,
                isNullable,
                namedTypeSymbol.TypeArguments.Select(arg => Resolve(arg, false)).ToArray());
        }

        return new TypeInfo(
            typeName,
            TypeRole.Dto,
            isNullable,
            []);
    }

    private static bool IsDictionary(INamedTypeSymbol typeSymbol) =>
        typeSymbol.Interfaces.Any(x => x.Name is "IDictionary" or "IReadOnlyDictionary");

    private static bool IsCollection(INamedTypeSymbol typeSymbol) => typeSymbol.Interfaces.Any(
        x => x.SpecialType is SpecialType.System_Array
            or SpecialType.System_Collections_Generic_ICollection_T
            or SpecialType.System_Collections_Generic_IReadOnlyCollection_T);

    private static bool IsNullableStruct(INamedTypeSymbol typeSymbol) =>
        typeSymbol.OriginalDefinition.SpecialType == SpecialType.System_Nullable_T;

    private static bool IsPrimitive(INamedTypeSymbol typeSymbol)
    {
        if (typeSymbol.SpecialType is
            SpecialType.System_Void or
            SpecialType.System_Boolean or
            SpecialType.System_Byte or
            SpecialType.System_SByte or
            SpecialType.System_Int16 or
            SpecialType.System_UInt16 or
            SpecialType.System_Int32 or
            SpecialType.System_UInt32 or
            SpecialType.System_Int64 or
            SpecialType.System_UInt64 or
            SpecialType.System_Single or
            SpecialType.System_Double or
            SpecialType.System_Decimal or
            SpecialType.System_Char or
            SpecialType.System_String)
        {
            return true;
        }

        return typeSymbol.ToDisplayString()
            is "DateTime"
            or "DateOnly"
            or "DateTimeOffset"
            or "TimeOnly"
            or "TimeSpan"
            or "Guid"
            or "Index"
            or "Range"
            or "Half"
            or "System.Numerics.BigInteger"
            or "System.Numerics.Complex";
    }
}