using ActionDetection.API.Infrastructure;
using ActionDetection.API.Infrastructure.Extensions;
using ActionDetection.API.Infrastructure.ObjectDetection;

using Microsoft.AspNetCore.Mvc;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

using System.Net.WebSockets;
using System.Text;

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

        [HttpGet]
        public async Task FrameSubscriptionAsync(string cameraIP,
                                   ImageSize imageSize,
                                   int sensitivity = 7,
                                   bool showMotion = true,
                                   int chunks = 64)
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                if (_cameras.Any(x => x.IPAddress == cameraIP))
                {
                    await ServeCameraFrames(webSocket, _cameras.FirstByIPAddress(cameraIP), imageSize, sensitivity, showMotion, chunks);
                }
            }
            else
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }

        private static async Task ServeCameraFrames(WebSocket webSocket, Camera camera, ImageSize imageSize, int sensitivity, bool showMotion, int chunks)
        {
            var buffer = new byte[1024 * 4];
            var receiveResult = await webSocket.ReceiveAsync(
                new ArraySegment<byte>(buffer), CancellationToken.None);

            while (webSocket.State == WebSocketState.Open)
            {
                var frame = await camera.GetMotionDetectionFrameAsync(imageSize, sensitivity, chunks);
                await webSocket.SendAsync(Encoding.UTF8.GetBytes(Convert.ToBase64String(frame.ToByteArray())), WebSocketMessageType.Text, true, CancellationToken.None);
                await Task.Delay(1000);
                //receiveResult = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }

            await webSocket.CloseAsync(
                receiveResult.CloseStatus.Value,
                receiveResult.CloseStatusDescription,
                CancellationToken.None);
        }
    }
}