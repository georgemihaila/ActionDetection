using ActionDetection.API.Infrastructure.Extensions;
using ActionDetection.API.Infrastructure.ObjectDetection;

using Microsoft.AspNetCore.Mvc;

using System.Drawing;
using System.Drawing.Imaging;

namespace ActionDetection.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class CameraController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ObjectDetectionService _objectDetectionService;
        private readonly HttpClient _httpClient = new();

        public CameraController(IConfiguration configuration, ObjectDetectionService objectDetectionService)
        {
            _configuration = configuration;
            _objectDetectionService = objectDetectionService;
        }

        [HttpGet]
        public IEnumerable<string> List()
        {
            return _configuration.GetSection("Cameras").GetChildren().Select(x => x.Value);
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
            Bitmap bmp = new(image.Pixels[0].Length, image.Pixels.Length);
            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    var p = image.Pixels[y][x];
                    bmp.SetPixel(x, y, Color.FromArgb(255, p[0], p[1], p[2]));
                }
            }
            return File(bmp.ToByteArray(), "image/jpeg");
        }

        [HttpGet]
        public async Task<IActionResult> GetFrame(string cameraIP, ImageSize imageSize)
        {
            //var bytes = await _httpClient.GetByteArrayAsync($"http://{cameraIP}/{imageSize.ToString().ToLower()}.jpg");
            Bitmap bitmap = new Bitmap(await _httpClient.GetStreamAsync($"http://{cameraIP}/{imageSize.ToString().ToLower()}.jpg"));
            return File(bitmap.ToByteArray(), "image/jpeg");
        }
    }
}