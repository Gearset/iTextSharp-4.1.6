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
    
    public struct Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
    
    public struct Color
    {
        // Taken from https://github.com/dotnet/corefx/blob/master/src/Common/src/System/Drawing/KnownColorTable.cs#L120 
        public static Color Gray = FromArgb(255, 128, 128, 128);
        
        public static Color FromArgb(int red, int green, int blue)
        {
            return FromArgb(255, red, green, blue);
        }
        
        public static Color FromArgb(int alpha, int red, int green, int blue)
        {
            CheckByte(alpha, nameof(alpha));
            CheckByte(red, nameof(red));
            CheckByte(green, nameof(green));
            CheckByte(blue, nameof(blue));

            var ret = new Color
            {
                A = (byte) alpha,
                R = (byte) red,
                G = (byte) green,
                B = (byte) blue,
            };
            return ret;
        }

        public byte A { get; set; }

        public byte R { get; set; }
        
        public byte G { get; set; }
        
        public byte B { get; set; }
        
        private static void CheckByte(int value, string name)
        {
            if (unchecked((uint) value) > byte.MaxValue)
                throw new ArgumentException("Invalid color");
        }

        public int ToArgb()
        {
            return 1;
        }
    }
}