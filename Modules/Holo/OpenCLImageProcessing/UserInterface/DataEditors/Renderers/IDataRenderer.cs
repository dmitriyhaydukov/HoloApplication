using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UserInterface.DataEditors.Tools;

namespace UserInterface.DataEditors.Renderers
{
    public interface IDataRenderer : IDisposable
    {
        Type DataType { get; }
        void Resize(Size size);

        object GetData();
        void SetData(object data);
        void Update();
        string GetTitle();  // TODO: заменить на OnTitleUpdate?
        IReadOnlyCollection<ITool> GetTools();

        event Action OnUpdateRequest;
        event Action UpdateControlsRequest;
    }
}