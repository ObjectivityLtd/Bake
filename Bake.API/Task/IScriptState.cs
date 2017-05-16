namespace Bake.API.Task
{
    public interface IScriptState
    {
        ITask CurrentTask { get; set; }
    }
}
