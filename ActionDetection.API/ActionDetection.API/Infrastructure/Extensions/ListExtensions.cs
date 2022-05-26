namespace ActionDetection.API.Infrastructure.Extensions
{
    public static class ListExtensions
    {
        public static Camera FirstByIPAddress(this IEnumerable<Camera> cameras, string ipAddress) => cameras.FirstOrDefault(x => x.IPAddress == ipAddress); 
    }
}
