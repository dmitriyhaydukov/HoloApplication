using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Processing;

namespace UserInterface.DataEditors.Renderers.Shaders
{
    public class SimpleShader : SimpleShaderWithProjectionViewModelMatricesBase, IImageShader
    {
        #region fragment shader
        protected override string FragmentShaderCode => @"
#version 410 core
in vec2 TexCoord;
out vec4 color;

uniform float top;
uniform float bottom;
uniform float min;
uniform float max;
uniform sampler2D ourTexture;

void main()
{
    color = texture(ourTexture, TexCoord);
    color = vec4(color.r, color.r, color.r, 1);
    //color = color.g > 0 ? vec4(1,0,0,1) : vec4(0,1,0,1);
    //color = (color - min) / (max - min);
    //color = (color - bottom) / (top - bottom);
}
";
        #endregion
       
        public void SetRange(float min, float max)
        {
            var minLocation = GL.GetUniformLocation(Id, "min");
            OpenGlErrorThrower.ThrowIfAny();
            var maxLocation = GL.GetUniformLocation(Id, "max");
            OpenGlErrorThrower.ThrowIfAny();
            GL.Uniform1(maxLocation, max);
            OpenGlErrorThrower.ThrowIfAny();
            GL.Uniform1(minLocation, min);
            OpenGlErrorThrower.ThrowIfAny();
        }
        public void SetViewRange(float bottom, float top)
        {
            if (bottom < 0 || top > 1 || bottom > top)
            {
                throw new InvalidOperationException();
            }

            var bottomLocation = GL.GetUniformLocation(Id, "bottom");
            OpenGlErrorThrower.ThrowIfAny();
            var topLocation = GL.GetUniformLocation(Id, "top");
            OpenGlErrorThrower.ThrowIfAny();
            GL.Uniform1(topLocation, top);
            OpenGlErrorThrower.ThrowIfAny();
            GL.Uniform1(bottomLocation, bottom);
            OpenGlErrorThrower.ThrowIfAny();
        }

        public override void SetValueRange(FloatRange channel1Range, FloatRange channel2Range)
        {
        }
    }
}
