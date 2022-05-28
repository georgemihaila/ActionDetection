using ActionDetection.API.Infrastructure.Extensions;
using ActionDetection.API.Infrastructure.ObjectDetection;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace ActionDetection.API.Infrastructure
{
    public class Camera
    {
        public string IPAddress { get; private set; }
        public double MaxFrameRate { get; set; } = 3;

        private readonly HttpClient _httpClient = new();
        public Image? CurrentFrame { get; private set; }
        public Image? LastFrame { get; private set; }
        private DateTime _lastGetFrameTime = DateTime.MinValue;
        private bool _loadActive = false;

        public Camera(string ipAddress)
        {
            IPAddress = ipAddress.Replace("http://", string.Empty);
        }

        /// <summary>
        /// Returns a camera frame by making a GET request
        /// </summary>
        public async Task<Image> GetFrameAsync(ImageSize imageSize)
        {
            if ((DateTime.Now - _lastGetFrameTime).TotalSeconds > 1 / MaxFrameRate && !_loadActive)
            {
                _lastGetFrameTime = DateTime.Now;
                _loadActive = true;
                LastFrame = CurrentFrame?.CloneAs<Rgb24>();
                Console.WriteLine($"[{DateTime.Now.TimeOfDay.ToString()}] {IPAddress} get frame");
                var tokenFactory = new CancellationTokenSource();
                tokenFactory.CancelAfter(1000);
                var token = tokenFactory.Token;
                CurrentFrame = Image.Load(await _httpClient.GetStreamAsync($"http://{IPAddress}/{imageSize.ToString().ToLower()}.jpg", token));
                _loadActive = false;
            }
            return CurrentFrame;
        }

        public void SetCurrentFrame(Image frame)
        {
            LastFrame = CurrentFrame?.CloneAs<Rgb24>();
            CurrentFrame = frame;
        }

        public async Task<bool> StartStreamAsync() => await GETPathAndReturnSuccessCodeAsync("startStream");

        public async Task<bool> StopStreamAsync() => await GETPathAndReturnSuccessCodeAsync("stopStream");

        private async Task<bool> GETPathAndReturnSuccessCodeAsync(string path)
        {
            var response = await _httpClient.GetAsync($"http://{IPAddress}/{path}");
            return response.IsSuccessStatusCode;
        }
    }
}
