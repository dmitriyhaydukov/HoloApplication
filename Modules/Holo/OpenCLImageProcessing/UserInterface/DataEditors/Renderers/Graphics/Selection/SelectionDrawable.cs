using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Processing;

namespace UserInterface.DataEditors.Renderers.Graphics.Selection
{
    public class SelectionDrawable : ImagePlane
    {
        
        public ImageSelection Selection { get; set; }
        
        public override void Draw(ViewParametres viewParametres)
        {
            GL.Clear(ClearBufferMask.DepthBufferBit);   // why it works??!

            var modelMatrix = GetModelMatrix(viewParametres);
            var translationMatrix = Matrix4.CreateTranslation(0, 0, -5f);
            modelMatrix = translationMatrix * modelMatrix;
            _shader.Use();
            _shader.SetModelMatrix(modelMatrix);
            _shader.SetProjectionMatrix(viewParametres.ProjectionMatrix);
            _shader.SetViewMatrix(viewParametres.ViewMatrix);
            _shader.SetSelection(Selection);
            _shader.SetZoom((int)(1 / viewParametres.Zoom));
            _shader.SetTime((int)DateTime.Now.TimeOfDay.TotalMilliseconds / 500);
            _shader.SetImageSize(ImageHandler.Width, ImageHandler.Height);
            base.Draw(viewParametres);
        }

        /// <summary>
        /// Вызови этот метод при обновлении выделения. Он попросит рендерер перерисовать
        /// </summary>
        public void SelectionUpdate()
        {
            InvokeRedrawRequest();
        }
        
        private static SelectionShader _shader = new SelectionShader();
    }
}
