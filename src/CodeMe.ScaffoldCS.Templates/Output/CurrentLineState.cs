namespace CodeMe.ScaffoldCS.Templates.Output;

internal enum CurrentLineState
{
    Empty,

    ValueAppended,

    CrAppended,

    LfAppended,

    Closed
}