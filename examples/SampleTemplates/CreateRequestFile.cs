using CodeMe.ScaffoldCS;
using CodeMe.ScaffoldCS.Metadata;
using CodeMe.ScaffoldCS.Metadata.CodeModel;
using CodeMe.ScaffoldCS.Templates;
using CodeMe.ScaffoldCS.Templates.Render;
using Microsoft.Extensions.Options;
using static CodeMe.ScaffoldCS.Templates.Render.SymbolsCS;

namespace SampleTemplates;

public class CreateRequestFile(
    ICSharpModelProvider codeModelProvider,
    IOptions<ScaffoldOptions> options) : ScaffoldFileBase
{
    private DtoModel GetModel()
    {
        var model = codeModelProvider.GetDtoByFile(options.Value.ModelPath!);

        var fields = model.Fields
            .Where(
                x => !x.Name.StartsWith("Created")
                    && !x.Name.StartsWith("Updated"))
            .ToArray();

        model = model with
        {
            Name = $"Create{model.Name}Request",
            Fields = fields
        };

        return model;
    }

    protected override string FileName => "CreateRequest.cs";

    protected override void Render(Template template)
    {
        var model = GetModel();
        var fields = model.Fields;

        template.Write(
            $$"""
            namespace {{model.Namespace}};
            
            {{IF(model.Comment)}}
            /// <summary>
            /// {{model.Comment}}
            /// </summary>
            {{ENDIF}}
            {{
                fields.WhereHasValue(x => x.Comment)
                    .Render(x => $"""/// <param name="{x.Name}">{x.Comment}</param>""")
            }}
            public record {{model.Name}}({{
                fields.RenderCommaSeparated(x => $"{x.Type.Name} {x.Name}")
            }});
            """);
    }
}