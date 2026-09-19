using CodeMe.ScaffoldCS.Metadata.CodeModel;

namespace CodeMe.ScaffoldCS.Metadata;

public interface ICSharpMetadataSource
{
    public ValueTask<Entity> ReadAsync(string path, string? typeName, CancellationToken cancellation = default);
}