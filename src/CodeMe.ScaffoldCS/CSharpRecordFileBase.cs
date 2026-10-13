using CodeMe.ScaffoldCS.Metadata.CodeModel;
using CodeMe.ScaffoldCS.Templates;
using CodeMe.ScaffoldCS.Templates.Render;
using static CodeMe.ScaffoldCS.Templates.Render.SymbolsCS;

namespace CodeMe.ScaffoldCS;

public abstract class CSharpRecordFileBase : ScaffoldFileBase<DtoModel>
{
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
            {{fields
                .WhereHasValue(x => x.Comment)
                .Render(x => $"""/// <param name="{x.Name}">{x.Comment}</param>""")}}
            {{ENDIF}}
            public record {{Model.Name}}({{fields.RenderCommaSeparated(x => $"{x.Type.Name} {x.Name}")}});
            """);
    }
}