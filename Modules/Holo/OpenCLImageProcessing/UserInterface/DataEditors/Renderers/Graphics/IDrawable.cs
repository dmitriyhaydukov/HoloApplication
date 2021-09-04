using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using Processing;

namespace UserInterface.DataEditors.Renderers.Graphics
{
    //TODO: переработать IDrawable и всех наследников
    public interface IDrawable
    {
        event Action OnRedrawRequest;
        void Draw(ViewParametres viewParametres);
        void SetImage(IImageHandler imageHandler);
    }

    public abstract class DrawableBase: IDrawable
    {
        public event Action OnRedrawRequest;
        public abstract void Draw(ViewParametres viewParametres);

        protected IImageHandler ImageHandler;

        protected void InvokeRedrawRequest()
        {
            OnRedrawRequest?.Invoke();
        }

        public virtual void SetImage(IImageHandler imageHandler)
        {
            ImageHandler = imageHandler;
        }
    }
}
