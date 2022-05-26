using SixLabors.ImageSharp;

namespace ActionDetection.API.Infrastructure.Extensions
{
    public static class ImageExtensions
    {
        public static byte[] ToByteArray(this System.Drawing.Image imageIn)
        {

            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return ms.ToArray();
            }
        }

        public static byte[] ToByteArray(this Image imageIn)
        {

            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, SixLabors.ImageSharp.Formats.Jpeg.JpegFormat.Instance);
                return ms.ToArray();
            }
        }
    }
}
