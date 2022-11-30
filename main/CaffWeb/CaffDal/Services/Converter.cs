using CaffDal.Entities;
using SkiaSharp;

namespace CaffDal.Services
{
    public static class Converter
    {
        public static byte[] Convert(Image content, int quality)
        {
            var offset = 0;
            SKBitmap img = new(width: content.Width, height: content.Height);
            for (int i = 0; i < img.Height; i++)
            {
                for (int j = 0; j < img.Width; j++)
                {
                    img.SetPixel(j, i, new SKColor(content.Preview[offset], content.Preview[offset + 1], content.Preview[offset + 2]));
                    offset += 3;
                }
            }
            using MemoryStream stream = new();
            img.Encode(stream, SKEncodedImageFormat.Jpeg, quality);
            return stream.ToArray();
        }
    }
}
