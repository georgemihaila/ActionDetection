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
    }
}
