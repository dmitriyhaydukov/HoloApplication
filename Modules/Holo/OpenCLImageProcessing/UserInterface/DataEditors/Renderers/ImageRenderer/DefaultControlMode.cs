using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Processing.DataBinding;

namespace UserInterface.DataEditors.Renderers.ImageRenderer
{
    public class DefaultControlMode : IDataRendererControlMode
    {
        private ImageRenderer _renderer;
        public DefaultControlMode(ImageRenderer renderer)
        {
            _renderer = renderer;
        }

        [Action(Group = "Zoom", TooltipText = "По размеру окна")]
        public void ZoomFit()
        {
            _renderer.ZoomFit();
            _renderer.RequestUpdate();
        }

        [Action(Group = "Zoom", TooltipText = "По размеру изображения")]
        public void ZoomEqual()
        {
            _renderer.ZoomEquals();
            _renderer.RequestUpdate();
        }
    }
}