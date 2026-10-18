using System.Runtime.CompilerServices;
using CodeMe.ScaffoldCS.Templates.Output;

namespace CodeMe.ScaffoldCS.Templates.Internals;

public interface ITemplatePartWriter : ITemplateValueWriter
{
    void BeginConditionBranch(ConditionBranch branch, bool condition);

    void Write(Action<ITemplatePartWriter> callback);

    void Write(Func<TemplateString> callback);

    [OverloadResolutionPriority(-1)]
    void Write<T>(IEnumerable<T> values);
}