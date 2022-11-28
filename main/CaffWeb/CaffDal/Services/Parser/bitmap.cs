﻿using SkiaSharp;
using SkiaSharp.Views.Desktop;

namespace CaffDal.Services.Parser
{
    public static class Parser
    { 
    public static Tuple<byte, byte, byte> read_content(byte[] x, int o)
    {
        return Tuple.Create(x[o], x[o + 1], x[o + 2]);
    }

    public static byte[] display(Ciff content)
    {
        var offset = 0;
        SKBitmap img = new SKBitmap(width: content.Width, height: content.Height);
            for (int i = 0; i < img.Height; i++)
            {
                for (int j = 0; j< img.Width;j ++)
                {
                var color = read_content(content.RawCiff, offset);
                    img.SetPixel(j, i, new SKColor(red: color.Item1, green: color.Item2, blue: color.Item3));
                offset += 3;
                }
            }
            using (MemoryStream stream = new MemoryStream())
            {
                img.Encode(stream, SKEncodedImageFormat.Jpeg, Int32.MaxValue);
                return stream.ToArray();
            }

        }
  }
}
