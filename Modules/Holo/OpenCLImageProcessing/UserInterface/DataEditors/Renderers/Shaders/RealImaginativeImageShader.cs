using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Processing;
using Processing.DataBinding;

namespace UserInterface.DataEditors.Renderers.Shaders
{
    [TargetImage(ImageFormat = ImageFormat.RealImaginative)]
    public class RealImaginativeImageShader : SimpleShaderWithProjectionViewModelMatricesBase
    {
        protected override string FragmentShaderCode => @"
#version 410 core
#define M_PI 3.1415926535897932384626433832795
in vec2 TexCoord;
out vec4 color;

uniform int mode;
uniform float offset;
uniform float multiplier;
uniform float minReal;
uniform float maxReal;
uniform float minImaginative;
uniform float maxImaginative;
uniform sampler2D ourTexture;

void main()
{
    color = texture(ourTexture, TexCoord); // r - real, g - imaginative
    float val;
    float real = color.r;
    float imag = color.g;
    //real = (real - minReal) / (maxReal - minReal);
    //imag = (imag - minImaginative) / (maxImaginative - minImaginative);  

    if (mode == 0) // amplitude
    {
        float maxR = max(abs(maxReal), abs(minReal));
        float maxI = max(abs(maxImaginative), abs(minImaginative));
        float maxAmplitude = sqrt(maxR*maxR + maxI*maxI);
        val = sqrt(real * real + imag * imag); 
        val = val / maxAmplitude;
    }
    else if (mode == 1) // phase
    {
        val = (atan(color.g, color.r) + M_PI) / (M_PI * 2);
    }
    else if (mode == 2) // real
    {
        val = (real - minReal) / (maxReal - minReal);
        //val = real;
    }
    else // imaginative
    {
        val = (imag - minImaginative) / (maxImaginative - minImaginative);
        //val = imag;
    }   
    
    val = val * multiplier + offset;
    color = vec4(val, val, val, 1);
}
";

        private FloatRange _realRange;
        private FloatRange _imaginativeRange;

        public enum ViewMode
        {
            Amplitude = 0,
            Phase = 1,
            Real = 2,
            Imaginative = 3
        }

        [EnumRadioGroup("Mode")]
        public AmplitudePhaseImageShader.ViewMode Mode { get; set; }

        [Number("Offset", -1, 1, 0.01f)]
        public float Offset { get; set; } = 0;

        [Number("Multiplier", -1000, 1000, 0.0001f)]
        public float Multiplier { get; set; } = 1;

        [Checkbox("Отразить по вертикали")]
        public bool MirrorVertical { get; set; }

        [Checkbox("Отразить по горизонтали")]
        public bool MirrorHorizontal { get; set; }

        [Checkbox("Циклический сдвиг")]
        public bool CyclicShift { get; set; }

        public override void Use()
        {
            base.Use();

            SetUniform1("offset", Offset);
            SetUniform1("multiplier", Multiplier);
            SetUniform1("mode", (int) Mode);

            SetUniform1("minReal", _realRange.Min);
            SetUniform1("maxReal", _realRange.Max);

            SetUniform1("minImaginative", _imaginativeRange.Min);
            SetUniform1("maxImaginative", _imaginativeRange.Max);

            SetUniform1("mirrorH", MirrorHorizontal ? 1 : 0);
            SetUniform1("mirrorV", MirrorVertical ? 1 : 0);
            SetUniform1("cyclicShift", CyclicShift ? 1 : 0);
        }


        public override void SetValueRange(FloatRange realRange, FloatRange imaginativeRange)
        {
            _realRange = realRange;
            _imaginativeRange = imaginativeRange;
        }

    }
}