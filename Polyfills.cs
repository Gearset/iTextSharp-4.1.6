using System.Runtime.CompilerServices;

namespace System.Drawing.Drawing2D
{
    public class Matrix
    {
        
    }
}
namespace System.Drawing.Imaging
{
    public class ImageFormat
    {
        
    }
}

namespace System.Drawing
{
    public class Bitmap : Image
    {
        private readonly int _width;
        private readonly int _height;
        private Color[,] _bitmap;

        public Bitmap(int width, int height)
        {
            _width = width;
            _height = height;
            _bitmap = new Color[width, height];
        }

        public int Width => _width;
        
        public int Height => _height;

        public Color GetPixel(int p0, int p1)
        {
            return _bitmap[p0, p1];
        }

        public void SetPixel(int p0, int p1, Color p2)
        {
            _bitmap[p0, p1] = p2;
        }
    }
    
    public class Image
    {
        
    }
}