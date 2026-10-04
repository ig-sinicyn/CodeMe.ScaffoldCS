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
        var model = codeModelProvider.GetDto(options.Value.ModelPath!);

        template.Write(
            $$"""
            namespace {{model.Namespace}};
            
            {{IF(model.Comment)}}
            /// <summary>
            /// Account details.
            /// </summary>
            {{ENDIF}}
            {{model.Fields.WhereHasValue(x => x.Comment).Render(x => $"""/// <param name="{x.Name}">{x.Comment}</param>""")}}
            public record Create{{model.Name}}Request({{model.Fields.RenderCommaSeparated(x => $"""{x.Type.Name} {x.Name}""")}});
            """);
    }
}