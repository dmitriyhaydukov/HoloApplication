using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using OpenTK;

namespace UserInterface.DataEditors.Renderers.Shaders
{
    public abstract class SimpleShaderWithProjectionViewModelMatricesBase: CustomizableShader, IImageShader
    {
        protected abstract string FragmentShaderCode { get; }

        #region vertex shader

        private string _vertexShaderCode = @"
#version 410 core
layout (location = 0) in vec3 position;
layout (location = 1) in vec2 texCoord;

out vec2 TexCoord;

uniform mat4 view;
uniform mat4 model;
uniform mat4 projection;
uniform int mirrorH;
uniform int mirrorV;
uniform int cyclicShift;

void main()
{
    gl_Position = projection * view * model * vec4(position, 1.0f);
    float u = texCoord.x;
    float v = texCoord.y;
    
    if (cyclicShift > 0)
    {
        u = u - 0.5;
        v = v - 0.5;        
    }

    if (mirrorH > 0)
        u = 1 - u;

    if (mirrorV > 0)
        v = 1 - v;

    TexCoord = vec2(u, v);
}
";
        #endregion
        public SimpleShaderWithProjectionViewModelMatricesBase()
        {
            Compile(_vertexShaderCode, FragmentShaderCode);
        }
        public void SetViewMatrix(Matrix4 view)
        {
            SetMatrix(view, "view");
        }
        public void SetProjectionMatrix(Matrix4 projection)
        {
            SetMatrix(projection, "projection");
        }
        public void SetModelMatrix(Matrix4 model)
        {
            SetMatrix(model, "model");
        }

        public abstract void SetValueRange(FloatRange channel1Range, FloatRange channel2Range);
    }
}
