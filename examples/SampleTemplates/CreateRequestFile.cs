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
    IOptions<ScaffoldOptions> options) : ScaffoldFileBase<DtoModel>
{
    protected override DtoModel CreateModel()
    {
        var model = codeModelProvider.GetDtoByFile(options.Value.ModelPath!);

        var fields = model.Fields
            .Where(
                x => !x.Name.StartsWith("Created")
                    && !x.Name.StartsWith("Updated"))
            .ToArray();

        model = model with
        {
            Name = $"Create{model.Name.TrimSuffix("Info")}Request",
            Fields = fields
        };

        return model;
    }

    protected override string FileName => $"{Model.Name}.cs";

    protected override void Render(Template template)
    {
        var fields = Model.Fields;

        template.Write(
            $$"""
            namespace {{Model.Namespace}};
            
            {{IF(Model.Comment)}}
            /// <summary>
            /// {{Model.Comment}}
            /// </summary>
            {{ENDIF}}
            {{
                fields
                    .WhereHasValue(x => x.Comment)
                    .Render(x => $"""/// <param name="{x.Name}">{x.Comment}</param>""")
            }}
            public record {{Model.Name}}({{
                fields.RenderCommaSeparated(x => $"{x.Type.Name} {x.Name}")
            }});
            """);
    }
}