using CodeMe.ScaffoldCS.Metadata.CodeModel;

namespace CodeMe.ScaffoldCS.Metadata;

public interface ICSharpMetadataProvider
{
    DtoModel GetDto(string typeName);

    DtoModel GetDto(string fileName, string typeName);
}