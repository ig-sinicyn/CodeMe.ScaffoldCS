using CodeMe.ScaffoldCS;
using CodeMe.ScaffoldCS.Metadata;
using CodeMe.ScaffoldCS.Metadata.CodeModel;
using CodeMe.ScaffoldCS.Templates.Render;
using Microsoft.Extensions.Options;

namespace SampleTemplates;

public class UpdateRequestFile(ICSharpModelProvider codeModelProvider, IOptions<ScaffoldOptions> options)
    : CSharpRecordFileBase
{
    protected override DtoModel CreateModel()
    {
        var model = codeModelProvider.GetDtoByFile(options.Value.ModelPath!);
        var comment = model.Comment?.TrimEnd('.');
        comment = string.IsNullOrEmpty(comment)
            ? model.Comment
            : $"{comment} (update request).";

        var fields = model.Fields
            .Where(
                x => !x.Name.StartsWith("Created")
                    && !x.Name.StartsWith("Updated"))
            .ToArray();

        model = model with
        {
            Name = $"Update{model.Name.TrimSuffix("Info")}Request",
            Comment = comment,
            Fields = fields
        };

        return model;
    }
}