using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Processing;

namespace UserInterface.DataEditors.Renderers.Shaders
{
    public class CustomizableShader: IShader
    {
        public int Id { get; private set; }

        protected CustomizableShader()
        {

        }
        public CustomizableShader(string vertexShaderCode, string fragmentShaderCode)
        {
            Compile(vertexShaderCode, fragmentShaderCode);
        }

        protected void Compile(string vertexShaderCode, string fragmentShaderCode)
        {
            var vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, vertexShaderCode);
            GL.CompileShader(vertexShader);
            var err = GL.GetError();
            var info = GL.GetShaderInfoLog(vertexShader);
            if (info != "")
                throw new Exception($"Error compiling {nameof(vertexShader)}.\r\nMessage:\r\n{info}");

            var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, fragmentShaderCode);
            GL.CompileShader(fragmentShader);
            err = GL.GetError();
            info = GL.GetShaderInfoLog(fragmentShader);
            if (info != "")
                throw new Exception($"Error compiling {nameof(fragmentShader)}.\r\nMessage:\r\n{info}");

            var shaderProgram = GL.CreateProgram();
            GL.AttachShader(shaderProgram, vertexShader);
            GL.AttachShader(shaderProgram, fragmentShader);
            GL.LinkProgram(shaderProgram);
            err = GL.GetError();
            info = GL.GetProgramInfoLog(shaderProgram);
            if (info != "")
                throw new Exception($"Error compiling {nameof(shaderProgram)}.\r\nMessage:\r\n{info}");

            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);

            Id = shaderProgram;
        }

        public virtual void Use()
        {
            GL.UseProgram(Id);
        }

        public void SetMatrix(Matrix4 matrix, string location)
        {
            var matrixLocation = GL.GetUniformLocation(Id, location);
            OpenGlErrorThrower.ThrowIfAny();
            GL.UniformMatrix4(matrixLocation, false, ref matrix);
            OpenGlErrorThrower.ThrowIfAny();
        }

        protected void SetUniform1(string name, float value)
        {
            var location = GL.GetUniformLocation(Id, name);
            GL.Uniform1(location, value);
            OpenGlErrorThrower.ThrowIfAny();
        }

        protected void SetUniform1(string name, int value)
        {
            var location = GL.GetUniformLocation(Id, name);
            GL.Uniform1(location, value);
            OpenGlErrorThrower.ThrowIfAny();
        }
    }
}
