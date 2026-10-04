using CodeMe.ScaffoldCS;
using CodeMe.ScaffoldCS.Metadata;
using CodeMe.ScaffoldCS.Templates;
using CodeMe.ScaffoldCS.Templates.Render;
using Microsoft.Extensions.Options;
using static CodeMe.ScaffoldCS.Templates.Render.SymbolsCS;

namespace SampleTemplates;

public class CreateRequestFile(
    ICSharpModelProvider codeModelProvider,
    IOptions<ScaffoldOptions> options) : ScaffoldFileBase
{
    protected override string FileName => "CreateRequest.cs";

    protected override void Render(Template template)
    {
        var model = codeModelProvider.GetDtoByFile(options.Value.ModelPath!);

        var fields = model.Fields
            .Where(
                x => !x.Name.StartsWith("Created")
                    && !x.Name.StartsWith("Updated"))
            .ToArray();

        template.Write(
            $$"""
            namespace {{model.Namespace}};
            
            {{IF(model.Comment)}}
            /// <summary>
            /// {{model.Comment}}
            /// </summary>
            {{ENDIF}}
            {{fields.WhereHasValue(x => x.Comment).Render(x => $"""/// <param name="{x.Name}">{x.Comment}</param>""")}}
            public record Create{{model.Name}}Request({{fields.RenderCommaSeparated(x => $"""{x.Type.Name} {x.Name}""")}});
            """);
    }
}