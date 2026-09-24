using CodeMe.ScaffoldCS.Metadata.CodeModel;

namespace CodeMe.ScaffoldCS.Metadata.CSharp;

public interface ICSharpMetadataContext
{
    DtoModel GetDto(string fileName, string typeName);
}