using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Processing.DataBinding;

namespace Processing.DataProcessors
{
    //[DataProcessor(Name = "Обесцвечивание", Group = "Common", Tooltip = "Преобразование цветного изображения в черно-белое")]
    public class RgbToGreyscaleConverter//: IDataProcessor
    {
        [Input("Изображение для обработки")]
        [ImageSlot("Изображение для обработки", "Default", ImageFormat.RGB, ImageFormat.RGBA)]
        public IImageHandler Input { get; set; }

        [Number("Коэффициент Красного канала", 0.01f, 0, 1)]
        public float RedFactor { get; set; } = 0.3f;

        [Number("Коэффициент Зеленого канала", 0.01f, 0, 1)]
        public float GreenFactor { get; set; } = 0.59f;

        [Number("Коэффициент Синего канала", 0.01f, 0, 1)]
        public float BlueFactor { get; set; } = 0.11f;
        
        [Output("Обработанное изображение")]
        public IImageHandler Converted { get; set; }

        public void Initialize()
        {
           
        }

        public void InputUpdated(PropertyInfo propertyInfo)
        {
            throw new NotImplementedException();
        }
    }
}
