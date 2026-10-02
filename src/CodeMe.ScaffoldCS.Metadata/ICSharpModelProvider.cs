using CodeMe.ScaffoldCS.Metadata.CodeModel;

namespace CodeMe.ScaffoldCS.Metadata;

public interface ICSharpModelProvider
{
    DtoModel GetDto(string typeName);

    DtoModel GetDto(string fileName, string typeName);
}