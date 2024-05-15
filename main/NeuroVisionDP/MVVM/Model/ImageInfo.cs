using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace NeuroVisionDP.MVVM.Model
{
    public class ImageInfo
    {
        public BitmapImage Image { get; set; }
        public string FileName { get; set; }

        // Конструктор по умолчанию
        public ImageInfo() { }

        // Конструктор с параметрами
        public ImageInfo(BitmapImage image, string fileName)
        {
            Image = image;
            FileName = fileName;
        }
    }
}
