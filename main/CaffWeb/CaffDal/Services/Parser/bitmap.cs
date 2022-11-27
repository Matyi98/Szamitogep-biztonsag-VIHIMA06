using CaffDal.Entities;
using System.Drawing;


namespace CaffDal.Services.Parser
{
    public static class Parser
    { 
    public static Tuple<byte, byte, byte> read_content(byte[] x, int o)
    {
        return Tuple.Create(x[o], x[o + 1], x[o + 2]);
    }

    public static object display(Ciff content, bool show)
    {
        var offset = 0;
            //var img = new Bitmap Image.new("RGB", (content.Width, content.Height), "magenta");
             ImageMagick.MagickImage = new MagickImage("alma", )
           
        var pixels = img.load();
        foreach (var j in Enumerable.Range(0, img.size[1]))
        {
            foreach (var i in Enumerable.Range(0, img.size[0]))
            {
                pixels[i, j] = read_content(content.RawCiff, offset);
                offset += 3;
            }
        }
        if (show)
        {
            img.show();
        }
    }
  }
}
