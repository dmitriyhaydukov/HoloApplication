using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Processing;
using UserInterface.DataEditors.Renderers.Shaders;

namespace UserInterface.DataEditors.Renderers.Graphics.Selection
{
    public class SelectionShader : SimpleShaderWithProjectionViewModelMatricesBase
    {
        protected override string FragmentShaderCode => @"
#version 410 core
in vec2 TexCoord;
out vec4 color;

uniform sampler2D ourTexture;
uniform int x0;
uniform int y0;
uniform int x1;
uniform int y1;
uniform int iw;
uniform int ih;
uniform int zoom;
uniform int time;

void main()
{
    //color = texture(ourTexture, TexCoord); // r - amplitude, g - phase
    int cx = int(floor(TexCoord.x * iw));
    int cy = int(floor(TexCoord.y * ih));

    float a = 0;
    float c = 0;
    int halfLineWidth = zoom * 2;

    if (cy > y0 && cy < y1 && ((cx > x0 - halfLineWidth && cx < x0 + halfLineWidth) || (cx > x1 - halfLineWidth && cx < x1 + halfLineWidth)))
    {
        a = 1;
        c = ((cy + cx / zoom + time * 3) / (6 * zoom)) % 2;
    }
    else if (cx > x0 && cx < x1 && ((cy > y0 - halfLineWidth && cy < y0 + halfLineWidth) || (cy > y1 - halfLineWidth && cy < y1 + halfLineWidth)))
    {
        a = 1;
        c = ((cx + cy / zoom + time * 3) / (6 * zoom)) % 2;
    }
    else if (cx < x0 || cy < y0 || x1 > 0 && cx > x1 || y1 > 0 && cy > y1)
    {
        a = 0.5;
    }

    
        
    color = vec4(c, c, c, a);
}
";
        public override void SetValueRange(FloatRange channel1Range, FloatRange channel2Range)
        {
        }

        public void SetSelection(ImageSelection selection)
        {
            SetUniform1("x0", selection.X0);
            SetUniform1("x1", selection.X1);
            SetUniform1("y0", selection.Y0);
            SetUniform1("y1", selection.Y1);
        }

        public void SetImageSize(int width, int height)
        {
            SetUniform1("iw", width);
            SetUniform1("ih", height);
        }

        public void SetZoom(int zoom)
        {
            SetUniform1("zoom", zoom);
        }

        public void SetTime(int time)
        {
            SetUniform1("time", time);
        }
    }
}
