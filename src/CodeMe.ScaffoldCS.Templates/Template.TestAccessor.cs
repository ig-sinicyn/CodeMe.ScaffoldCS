using CodeMe.ScaffoldCS.Templates.Output;

namespace CodeMe.ScaffoldCS.Templates;

public partial class Template
{
    internal TestAccessor GetTestAccessor() => new(this);

    internal readonly struct TestAccessor(Template owner)
    {
        private readonly TemplateTextWriter.TestAccessor _writerAccessor = owner._writer.GetTestAccessor();

        public TextWriter Output => _writerAccessor.Output;

        public IReadOnlyCollection<TemplateTextWriterOptions> OptionsStack => owner._optionsStack;

        public BranchState CurrentBranch => owner.CurrentBranch;

        public IReadOnlyCollection<BranchState> BranchesStack => owner._branchesStack;
    }
}