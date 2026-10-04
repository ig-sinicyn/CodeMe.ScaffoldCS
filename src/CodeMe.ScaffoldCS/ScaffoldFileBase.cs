using System.Globalization;
using CodeMe.ScaffoldCS.Templates;
using CodeMe.ScaffoldCS.Templates.Output;

namespace CodeMe.ScaffoldCS;

public abstract class ScaffoldFileBase : IScaffoldPart
{
    protected virtual bool Enabled => true;

    protected abstract string FileName { get; }

    protected IScaffoldContext Context { get; private set; } = null!;

    public async ValueTask RenderAsync(IScaffoldContext context, CancellationToken cancellation = default)
    {
        if (!Enabled)
        {
            return;
        }

        var prev = Context;
        try
        {
            Context = context;
            var file = await context.GetOrCreateFileAsync(FileName, cancellation);
            var options = CreateTemplateOptions(context.RenderOptions);

            var output = new StringWriter();
            using (var template = new Template(output, options))
            using (template.BeginAmbientScope())
            {
                Render(template);
            }

            file.Content = output.ToString();
        }
        finally
        {
            Context = prev;
        }
    }

    protected virtual TemplateTextWriterOptions CreateTemplateOptions(ScaffoldRenderOptions options)
    {
        var result = TemplateTextWriterOptions.Default;
        result = result with
        {
            Culture = options.CultureName == null ? result.Culture : CultureInfo.GetCultureInfo(options.CultureName),
            NewLineFormat = options.NewLineFormat,
            NormalizeNewLines = true
        };
        return result;
    }

    protected abstract void Render(Template template);
}