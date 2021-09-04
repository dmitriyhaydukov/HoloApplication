using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Processing;
using Processing.DataBinding;

namespace UserInterface.DataEditors.Renderers.Shaders
{
    [TargetImage(ImageFormat = ImageFormat.RGB)]
    public class RGBImageShader : SimpleShaderWithProjectionViewModelMatricesBase
    {
        protected override string FragmentShaderCode => @"
#version 410 core
in vec2 TexCoord;
out vec4 color;

uniform int mode;
uniform float offset;
uniform float multiplier;
uniform sampler2D ourTexture;

void main()
{
    color = texture(ourTexture, TexCoord); 
}
";

        public override void SetValueRange(FloatRange channel1Range, FloatRange channel2Range)
        {
            
        }
    }
}