namespace CodeMe.ScaffoldCS;

public abstract class ScaffoldFileBase<TModel> : ScaffoldFileBase
{
    protected TModel Model { get; private set; } = default!;

    protected abstract TModel CreateModel();

    protected override void BeforeRender()
    {
        base.BeforeRender();
        Model = CreateModel();
    }
}