using Newtonsoft.Json;

namespace ActionDetection.API.Infrastructure.ObjectDetection
{
    public class ObjectDetectionService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly string _baseURL;

        public ObjectDetectionService(IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new HttpClient();
            _baseURL = configuration.GetValue<string>("ObjectDetectionServiceAddress");
        }

        public async Task<ObjectDetectionResponse> DetectObjectsInCameraViewAsync(string cameraIP, ImageSize imageSize)
        {
#pragma warning disable CS8603 // Possible null reference return.
            var responseString = await _httpClient.GetStringAsync($"{_baseURL}?ip={cameraIP}&size={imageSize.ToString().ToLower()}&type=json");
            var obj = JsonConvert.DeserializeObject<DetectedObject[]>(responseString);
            return new ObjectDetectionResponse()
            {
                DetectedObjects = obj
            };
#pragma warning restore CS8603 // Possible null reference return.
        }
    }
}
