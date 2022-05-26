using ActionDetection.API.Infrastructure.ObjectDetection;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

using System.Runtime.Serialization.Formatters.Binary;

namespace ActionDetection.API.Infrastructure.Extensions
{
    public static class ImageExtensions
    {
        public static T DeepClone<T>(this T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
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

        private static Image Resize(this Image source, int width, int height)
        {
            source.Mutate(x => x.Resize(width, height, KnownResamplers.Lanczos3));
            return source;
        }

        private static Image PrepareForMotionDetection(this Image source, int size = 16)
        {
            source = source.Resize(size, size);
            source.Mutate(x => x.Grayscale());
            return source;
        }

        public static async Task<Image> GetMotionDetectionFrameAsync(this Camera camera, ImageSize imageSize, int sensitivity)
        {
            var lastFrame = camera.GetLastFrame();
            var frame = await camera.GetFrameAsync(imageSize);
            if (lastFrame != null && frame != null)
            {
                var c0 = frame.PrepareForMotionDetection().CloneAs<Rgb24>();
                var c1 = lastFrame.PrepareForMotionDetection().CloneAs<Rgb24>();
                Image<Rgba32> result = new(c0.Width, c0.Height);
                for (int x = 0; x < c0.Width; x++)
                {
                    for (int y = 0; y < c0.Height; y++)
                    {
                        if (Math.Abs(c0[x, y].R - c1[x, y].R) > sensitivity)
                        {
                            result[x, y] = Color.Red;
                        }
                    }
                }
                return result;
            }
            return frame;
        }
    }
}
