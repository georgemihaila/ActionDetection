using ActionDetection.API.Infrastructure.ObjectDetection;

using Microsoft.AspNetCore.Mvc;

namespace ActionDetection.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class CameraController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ObjectDetectionService _objectDetectionService;

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
    }
} 