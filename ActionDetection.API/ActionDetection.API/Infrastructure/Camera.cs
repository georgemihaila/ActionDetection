using ActionDetection.API.Infrastructure.ObjectDetection;

using SixLabors.ImageSharp;

namespace ActionDetection.API.Infrastructure
{
    public class Camera
    {
        public string IPAddress { get;private set; }
        public int MaxFrameRate { get; set; } = 1;

        private readonly HttpClient _httpClient = new();
        private Image _lastImage;
        private DateTime _lastFrameTime = DateTime.MinValue;
        private bool _loadActive = false;

        public Camera(string ipAddress)
        {
            IPAddress = ipAddress.Replace("http://", string.Empty);
        }

        public async Task<Image> GetFrameAsync(ImageSize imageSize)
        {
            if ((DateTime.Now - _lastFrameTime).TotalSeconds > 1 / MaxFrameRate && !_loadActive)
            {
                _lastFrameTime = DateTime.Now;
                _loadActive = true;
                _lastImage = Image.Load(await _httpClient.GetStreamAsync($"http://{IPAddress}/{imageSize.ToString().ToLower()}.jpg"));
                _loadActive = false;
            }
            return _lastImage;
        }
    }
}
