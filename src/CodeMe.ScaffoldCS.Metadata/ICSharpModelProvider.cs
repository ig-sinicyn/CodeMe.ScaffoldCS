using CodeMe.ScaffoldCS.Metadata.CodeModel;

namespace CodeMe.ScaffoldCS.Metadata;

public interface ICSharpModelProvider
{
    DtoModel GetDtoByType(string typeName);

    DtoModel GetDtoByFile(string fileName, string? typeName = null);
}