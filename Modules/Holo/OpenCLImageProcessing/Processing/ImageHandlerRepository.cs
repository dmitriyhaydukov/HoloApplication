using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure;

namespace Processing
{
    public class ImageHandlerRepository : RepositoryBase<IImageHandler>
    {
        public void FreeResources()
        {
            foreach (var imageHandler in GetAll())
            {
                imageHandler.FreeComputingDevice();
            }
        }
    }
}
