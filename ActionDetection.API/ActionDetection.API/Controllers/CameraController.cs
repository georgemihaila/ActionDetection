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
    public class CameraController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ObjectDetectionService _objectDetectionService;
        private readonly CameraCollection _cameras;

        public CameraController(IConfiguration configuration, ObjectDetectionService objectDetectionService, CameraCollection cameras)
        {
            _configuration = configuration;
            _objectDetectionService = objectDetectionService;
            _cameras = cameras;
        }

        [HttpGet]
        public IEnumerable<string> List() => _cameras.Select(x => x.IPAddress);

        [HttpGet]
        public async Task<IActionResult> GetFrame(string cameraIP,
                                                  ImageSize imageSize,
                                                  int sensitivity = 7,
                                                  bool showMotion = true,
                                                  int chunks = 64)
        {
            var camera = _cameras.FirstByIPAddress(cameraIP);
            var bitmap = default(Image);
            if (showMotion)
            {
                bitmap = await camera.GetMotionDetectionFrameAsync(imageSize, sensitivity, chunks);
            }
            else
            {
                bitmap = await camera.GetFrameAsync(imageSize);
            }
            return File(bitmap.ToByteArray(), "image/jpeg");
        }
    }
}