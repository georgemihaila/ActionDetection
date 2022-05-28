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
                    await ServeCameraFramesAsync(webSocket, _cameras.FirstByIPAddress(cameraIP), imageSize, sensitivity, showMotion, chunks);
                }
            }
            else
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }

        private static async Task ServeCameraFramesAsync(WebSocket webSocket, Camera camera, ImageSize imageSize, int sensitivity, bool showMotion, int chunks)
        {
            var buffer = new byte[1024 * 4];
            try
            {
                var setResolutionResponse = await camera.SetStreamResolutionAsync(imageSize);
                var setFPSResponse = await camera.SetLowFPSAsync();
                var receiveResult = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                var lastHeardFrom = DateTime.Now;
                while (webSocket.State == WebSocketState.Open)
                {
                    if (!camera.IsStreaming)
                    {
                        await camera.StartStreamAsync();
                    }
                    var frame = camera.GetMotionDetectionFrame(sensitivity, chunks);
                    if (frame != null)
                    {
                        await webSocket.SendAsync(Encoding.UTF8.GetBytes(Convert.ToBase64String(frame.ToByteArray())), WebSocketMessageType.Text, true, CancellationToken.None);
                    }
                    await Task.Delay(500);

                    CancellationTokenSource source = new();
                    source.CancelAfter(TimeSpan.FromSeconds(5));
                    receiveResult = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), source.Token);
                    if (buffer?.Length > 0)
                    {
                        if (Encoding.UTF8.GetString(buffer).Contains("client up"))
                        {
                            lastHeardFrom = DateTime.Now;
                        }
                    }
                    if (DateTime.Now - lastHeardFrom > TimeSpan.FromSeconds(5))
                    {
                        break;
                    }
                }
                await webSocket.CloseAsync(WebSocketCloseStatus.EndpointUnavailable, "Connection terminated by client", CancellationToken.None);
                Console.WriteLine($"{camera.IPAddress} WS connection closed");
                var streamStop = await camera.StopStreamAsync();
                if (!streamStop)
                {
                    throw new Exception($"Couldn't stop {camera.IPAddress} camera stream");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        [HttpPost]
        public IActionResult SetFrame()
        {
            var image = Image.Load(Request.Body);
            var sourceIP = Request.HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();
            if (sourceIP != null)
            {
                if (_cameras.Any(x => x.IPAddress == sourceIP))
                {
                    _cameras.FirstByIPAddress(sourceIP).SetCurrentFrame(image);
                }
            }
            else
            {
                return BadRequest("No IP address");
            }
            return StatusCode(201);
        }
    }
}