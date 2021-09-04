using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Processing.DataBinding;
using Processing.ImageReaders;
using UserInterface.DataEditors.Renderers.Shaders;

namespace UserInterface.DataEditors.Renderers.ImageRenderer
{
    // TODO: возможно этот класс бесполезен
    public class DefaultViewMode : IDataRendererViewMode
    {
        private ImageRenderer _imageRenderer;

        [MergeSubfields(OnPropertyChanged = "ShaderPropertiesChanged")]
        public IImageShader Shader { get; }
        
        public void SetViewParametres(IViewParametres viewParams)
        {
            Shader.SetProjectionMatrix(viewParams.ProjectionMatrix);
            Shader.SetViewMatrix(viewParams.ViewMatrix);
        }

        public DefaultViewMode(ImageRenderer renderer)
        {
            Shader = new SimpleShader();
            _imageRenderer = renderer;
        }

        public DefaultViewMode(ImageRenderer renderer, IImageShader shader)
        {
            Shader = shader;
            _imageRenderer = renderer;
        }

        public void ShaderPropertiesChanged()
        {
            _imageRenderer.RequestUpdate();
        }
    }
}
