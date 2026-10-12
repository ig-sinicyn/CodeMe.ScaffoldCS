using CodeMe.ScaffoldCS;
using CodeMe.ScaffoldCS.Metadata.CodeModel;
using CodeMe.ScaffoldCS.Templates;
using CodeMe.ScaffoldCS.Templates.Render;

namespace SampleTemplates;

public abstract class CSharpRecordFileBase : ScaffoldFileBase<DtoModel>
{
    protected override string FileName => $"{Model.Name}.cs";

    protected override void Render(Template template)
    {
        var fields = Model.Fields;

        template.Write(
            $$"""
            namespace {{Model.Namespace}};
            
            {{SymbolsCS.IF(Model.Comment)}}
            /// <summary>
            /// {{Model.Comment}}
            /// </summary>
            {{SymbolsCS.ENDIF}}
            {{fields
                .WhereHasValue(x => x.Comment)
                .Render(x => $"""/// <param name="{x.Name}">{x.Comment}</param>""")}}
            public record {{Model.Name}}({{fields.RenderCommaSeparated(x => $"{x.Type.Name} {x.Name}")}});
            """);
    }
}