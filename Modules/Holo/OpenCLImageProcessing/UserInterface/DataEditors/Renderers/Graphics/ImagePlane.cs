using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using Processing;
using OpenTK.Graphics.OpenGL;

namespace UserInterface.DataEditors.Renderers.Graphics
{
    public class ImagePlane: DrawableBase
    {
        private readonly float[] _vertices = {
            0.5f,  0.5f, 0.0f,   1.0f, 0.0f,   // Top Right
            0.5f, -0.5f, 0.0f,   1.0f, 1.0f,   // Bottom Right
            -0.5f, -0.5f, 0.0f,   0.0f, 1.0f,   // Bottom Left
            -0.5f,  0.5f, 0.0f,   0.0f, 0.0f    // Top Left 
        };

        private readonly uint[] _indices = {  // Note that we start from 0!
            0, 1, 3,   // First Triangle
            1, 2, 3    // Second Triangle
        };

        private int _vao;
        private int _vbo;
        private int _ebo;


        public ImagePlane()
        {
            _vao = GL.GenVertexArray();
            _vbo = GL.GenBuffer();
            _ebo = GL.GenBuffer();

            GL.BindVertexArray(_vao);

            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            GL.BindVertexArray(0);
        }

        public Matrix4 GetModelMatrix(ViewParametres viewParams)
        {
            var rel = ImageHandler.Height / viewParams.ViewportSize.Height;
            return Matrix4.CreateScale(viewParams.Zoom * rel * ImageHandler.GetRatio(), viewParams.Zoom * rel, 1);
        }

        public override void Draw(ViewParametres viewParametres)
        {
//            var rel = _texture.Size.Height / viewParams.ViewportSize.Height;
//            _model = Matrix4.CreateScale(viewParams.Zoom * rel * _texture.Ratio, viewParams.Zoom * rel, 1);
//            shader.SetModelMatrix(_model);

            if (ImageHandler?.OpenGlTextureId == null) // Try upload to videocard?
                return;
            
            GL.BindTexture(TextureTarget.Texture2D, ImageHandler.OpenGlTextureId.Value);
            GL.BindVertexArray(_vao);
            GL.DrawElements(BeginMode.Triangles, 6, DrawElementsType.UnsignedInt, 0);
            GL.BindVertexArray(0);
            GL.BindTexture(TextureTarget.Texture2D, 0);
            OpenGlErrorThrower.ThrowIfAny();
        }
    }
}
