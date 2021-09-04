using UserInterface.DataEditors.Renderers;

namespace UserInterface.DataEditors
{
    public interface IDataEditor
    {
        IDataRenderer Renderer { get; }
    }
}