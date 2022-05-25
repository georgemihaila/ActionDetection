namespace ActionDetection.API.Infrastructure.ObjectDetection
{
    public class ObjectDetectionResponse
    {
        public DetectedObject[] DetectedObjects { get; set; }
    }

    public class DetectedObject
    {
        public string name { get; set; }
        public float percentage_probability { get; set; }
        public int[] box_points { get; set; }
    }

}
