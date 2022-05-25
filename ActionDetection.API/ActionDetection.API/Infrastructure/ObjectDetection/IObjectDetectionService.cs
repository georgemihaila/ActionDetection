namespace ActionDetection.API.Infrastructure.ObjectDetection
{
    public interface IObjectDetectionService
    {
        public Task<ObjectDetectionResponse> DetectObjectsInCameraViewAsync(string cameraIP, ImageSize imageSize);
        public Task<ImageDetectionResponse> GetDetectionImageAsync(string cameraIP, ImageSize imageSize);
    }
}
