﻿using ActionDetection.API.Infrastructure.Extensions;
using ActionDetection.API.Infrastructure.ObjectDetection;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace ActionDetection.API.Infrastructure
{
    public class Camera
    {
        public string IPAddress { get; private set; }
        public int MaxFrameRate { get; set; } = 1;

        private readonly HttpClient _httpClient = new();
        private Image? _currentFrame;
        private Image? _lastFrame;
        private DateTime _lastGetFrameTime = DateTime.MinValue;
        private bool _loadActive = false;

        public Camera(string ipAddress)
        {
            IPAddress = ipAddress.Replace("http://", string.Empty);
        }

        public async Task<Image> GetFrameAsync(ImageSize imageSize)
        {
            if ((DateTime.Now - _lastGetFrameTime).TotalSeconds > 1 / MaxFrameRate && !_loadActive)
            {
                _lastGetFrameTime = DateTime.Now;
                _loadActive = true;
                _lastFrame = _currentFrame?.CloneAs<Rgb24>();
                _currentFrame = Image.Load(await _httpClient.GetStreamAsync($"http://{IPAddress}/{imageSize.ToString().ToLower()}.jpg"));
                _loadActive = false;
            }
            return _currentFrame;
        }

        public Image GetLastFrame() => _lastFrame;
    }
}