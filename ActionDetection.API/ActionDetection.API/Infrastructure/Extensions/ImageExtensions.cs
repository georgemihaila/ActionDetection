﻿using ActionDetection.API.Infrastructure.ObjectDetection;

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

        private static Image PrepareForMotionDetection(this Image source, int size)
        {
            source = source.CloneAs<Rgba32>().Resize(size, size);
            source.Mutate(x => x.Grayscale());
            return source;
        }

        public static async Task<Image> GetMotionDetectionFrameAsync(this Camera camera, ImageSize imageSize, int sensitivity, int chunks)
        {
            var lastFrame = camera.GetLastFrame();
            var frame = (await camera.GetFrameAsync(imageSize)).CloneAs<Rgb24>();
            Image<Rgba32> motionImage;
            if (lastFrame != null && frame != null)
            {
                var c0 = frame.PrepareForMotionDetection(chunks).CloneAs<Rgb24>();
                var c1 = lastFrame.PrepareForMotionDetection(chunks).CloneAs<Rgb24>();
                motionImage = new Image<Rgba32>(frame.Width, frame.Height);
                var adjustedChunkWidth = motionImage.Width / chunks;
                var adjustedChunkHeight= motionImage.Height / chunks;
                for (int chunkX = 0; chunkX < c0.Width; chunkX++)
                {
                    for (int chunkY = 0; chunkY < c0.Height; chunkY++)
                    {
                        var motionDetected = Math.Abs(c0[chunkX, chunkY].R - c1[chunkX, chunkY].R) > sensitivity;
                        var color = Color.Red;
                        if (!motionDetected)
                        {
                            color = Color.Transparent;
                        }
                        for (int i = 0; i < motionImage.Width / chunks; i++)
                        {
                            for (int j = 0; j < motionImage.Height / chunks; j++)
                            {
                                motionImage[i + chunkX * adjustedChunkWidth, j + chunkY * adjustedChunkHeight] = color;
                            }
                        }
                    }
                }
                //return motionImage;
                frame.Mutate(x => x.DrawImage(motionImage, 0.3f));
            }
            return frame;
        }
    }
}
