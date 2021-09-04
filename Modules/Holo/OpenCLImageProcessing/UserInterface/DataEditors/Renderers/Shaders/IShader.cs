using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using OpenTK;

namespace UserInterface.DataEditors.Renderers.Shaders
{
    public interface IShader
    {
        void Use();
    }

    public interface IImageShader: IShader
    {
        void SetViewMatrix(Matrix4 view);
        void SetProjectionMatrix(Matrix4 projection);
        void SetModelMatrix(Matrix4 model);
        void SetValueRange(FloatRange channel1Range, FloatRange channel2Range);
    }
}
