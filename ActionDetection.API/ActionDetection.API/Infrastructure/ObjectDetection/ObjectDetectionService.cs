using Newtonsoft.Json;

#pragma warning disable CS8603 // Possible null reference return.
namespace ActionDetection.API.Infrastructure.ObjectDetection
{
    public class ObjectDetectionService : IObjectDetectionService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly string _baseURL;

        private static Dictionary<string, (ObjectDetectionResponse, bool)> _objectDetectionCache = new Dictionary<string, (ObjectDetectionResponse, bool)>();
        private static Dictionary<string, (ImageDetectionResponse, bool)> _imageDetectionCache = new Dictionary<string, (ImageDetectionResponse, bool)>();

        public ObjectDetectionService(IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new HttpClient();
            _baseURL = configuration.GetValue<string>("ObjectDetectionServiceAddress");
        }

        public async Task<ObjectDetectionResponse> DetectObjectsInCameraViewAsync(string cameraIP, ImageSize imageSize)
        {
            if (!_objectDetectionCache.ContainsKey(cameraIP))
            {
                _objectDetectionCache.Add(cameraIP, (new ObjectDetectionResponse(), false));
            }

            var entry = _objectDetectionCache[cameraIP];
            if (!entry.Item2)
            {
                entry.Item2 = true;
                entry.Item1 = await Task.Run(async () =>
                {
                    try
                    {
                        var responseString = await _httpClient.GetStringAsync($"{_baseURL}?ip={cameraIP}&size={imageSize.ToString().ToLower()}&type=json");
                        var obj = JsonConvert.DeserializeObject<DetectedObject[]>(responseString);
                        return new ObjectDetectionResponse()
                        {
                            DetectedObjects = obj
                        };
                    }
                    finally
                    {
                        entry.Item2 = false;
                    }
                });
            }
            return entry.Item1;
        }
        public async Task<ImageDetectionResponse> GetDetectionImageAsync(string cameraIP, ImageSize imageSize)
        {
            if (!_imageDetectionCache.ContainsKey(cameraIP))
            {
                _imageDetectionCache.Add(cameraIP, (new ImageDetectionResponse(), false));
            }

            var entry = _imageDetectionCache[cameraIP];
            if (!entry.Item2)
            {
                entry.Item2 = true;
                entry.Item1 = await Task.Run(async () =>
                {
                    try
                    {
                        var responseString = await _httpClient.GetStringAsync($"{_baseURL}?ip={cameraIP}&size={imageSize.ToString().ToLower()}&type=image");
                        var obj = JsonConvert.DeserializeObject<int[][][]>(responseString);
                        return new ImageDetectionResponse()
                        {
                            Pixels = obj
                        };
                    }
                    finally
                    {
                        entry.Item2 = false;
                    }
                });
            }
            return entry.Item1;
        }
    }
}

#pragma warning restore CS8603 // Possible null reference return.