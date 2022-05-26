using ActionDetection.API.Infrastructure;
using ActionDetection.API.Infrastructure.Extensions;
using ActionDetection.API.Infrastructure.ObjectDetection;

using Microsoft.AspNetCore.Mvc;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace ActionDetection.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ObjectDetectionController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ObjectDetectionService _objectDetectionService;
        private readonly IEnumerable<Camera> _cameras;

        public ObjectDetectionController(IConfiguration configuration, ObjectDetectionService objectDetectionService, IEnumerable<Camera> cameras)
        {
            _configuration = configuration;
            _objectDetectionService = objectDetectionService;
            _cameras = cameras;
        }

        [HttpGet]
        public async Task<ObjectDetectionResponse> DetectObjectsInCameraViewAsync(string cameraIP, ImageSize imageSize)
        {
            return await _objectDetectionService.DetectObjectsInCameraViewAsync(cameraIP, imageSize);
        }
        
        [HttpGet]
        public async Task<IActionResult> GetDetectionImageAsync(string cameraIP, ImageSize imageSize)
        {
            var image = await _objectDetectionService.GetDetectionImageAsync(cameraIP, imageSize);
            Image<Rgba32> bmp = new(image.Pixels[0].Length, image.Pixels.Length);
            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    var p = image.Pixels[y][x];
                    bmp[x, y] = Color.FromRgba(255, (byte)p[0], (byte)p[1], (byte)p[2]);
                }
            }
            return File(bmp.ToByteArray(), "image/jpeg");
        }
    }
}