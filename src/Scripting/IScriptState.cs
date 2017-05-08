using Cake.CD.Templating;

namespace Cake.CD.Scripting
{
    public interface IScriptState
    {
        IScriptTask CurrentTask { get; set; }
    }
}
