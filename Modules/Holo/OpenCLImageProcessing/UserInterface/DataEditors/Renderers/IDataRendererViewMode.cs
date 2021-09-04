using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Processing;
using UserInterface.DataEditors.Renderers.Shaders;

namespace UserInterface.DataEditors.Renderers
{
    public interface IDataRendererViewMode
    {
        IImageShader Shader { get; }

        void SetViewParametres(IViewParametres viewParametres);
    }

    public class TargetImageAttribute : Attribute
    {
        public ImageFormat ImageFormat;
    }
}
