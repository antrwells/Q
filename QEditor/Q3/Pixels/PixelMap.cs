/// <summary>
/// A proxy bitmap namespace.
/// </summary>
namespace Q.Pixels
{

    /// <summary>
    /// A PixelMap is a proxy image. It is not tied to OpenGL or any other renderer.
    /// It merely contains the raw pixel data, with or without Alpha.
    /// </summary>
    public class PixelMap
    {
        /// <summary>
        /// The width of the pixelmap.
        /// </summary>
        public int Width;
        /// <summary>
        /// The height of the pixelmap.
        /// </summary>
        public int Height;
        /// <summary>
        /// If true, the bitmap has alpha data(4 bytes vs 3)
        /// </summary>
        public bool Alpha = true;
        /// <summary>
        /// The raw bitmap data. 0-255 per component, per pixel.
        /// </summary>
        public byte[] Data = null;
        private int pixelSize = 3;


        /// <summary>
        /// Creates a new blank pixelmap.
        /// </summary>
        /// <param name="w">The width of the pixelmap.</param>
        /// <param name="h">The height of the pixelmap.</param>
        /// <param name="alpha">If true, pixel is 4 bytes per pixel, if not, 3.</param>
        public PixelMap(int w, int h, bool alpha)
        {
            Width = w;
            Height = h;
            Alpha = alpha;
            if (alpha) pixelSize = 4;
            Data = new byte[w * h * pixelSize];
            for (int l = 0; l < Data.Length; l++)
            {
                Data[l] = 0;
            }
        }


        /// <summary>
        /// Gets the red component of a pixel.
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        /// <returns>R</returns>
        public byte GetR(int x, int y)
        {
            return Data[y * Width * pixelSize + (x * pixelSize)];
        }

        /// <summary>
        /// Gets the green component of a pixel.
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        /// <returns>G</returns>
        public byte GetG(int x, int y)
        {
            return Data[y * Width * pixelSize + (x * pixelSize) + 1];
        }


        /// <summary>
        /// Gets the blue component of a pixel.
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        /// <returns>B</returns>
        public byte GetB(int x, int y)
        {
            return Data[y * Width * pixelSize + (x * pixelSize) + 2];
        }

        /// <summary>
        /// Gets the Alpha component of a pixel.
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        /// <returns>A</returns>
        public byte GetA(int x, int y)
        {
            return Data[y * Width * pixelSize + (x * pixelSize) + 3];
        }

        /// <summary>
        /// Gets the RGBA of a given pixel.
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        /// <param name="R">R</param>
        /// <param name="G">G</param>
        /// <param name="B">B</param>
        /// <param name="A">A</param>
        public void GetRGB(int x, int y, ref byte R, ref byte G, ref byte B, ref byte A)
        {
            R = GetR(x, y); G = GetG(x, y); B = GetB(x, y); A = GetA(x, y);
        }

        /// <summary>
        /// Sets the RGBA of a given pixel.
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        /// <param name="r">R</param>
        /// <param name="g">G</param>
        /// <param name="b">B</param>
        /// <param name="a">A</param>
        public void SetRGB(int x, int y, byte r, byte g, byte b, byte a)
        {
            if (x < Width && y < Height)
            {
                Data[y * Width * pixelSize + (x * pixelSize)] = r;
                Data[y * Width * pixelSize + (x * pixelSize) + 1] = g;
                Data[y * Width * pixelSize + (x * pixelSize) + 2] = b;
                Data[y * Width * pixelSize + (x * pixelSize) + 3] = a;
            }
        }
    }
}