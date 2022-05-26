namespace ActionDetection.API.Infrastructure.Extensions
{
    public static class ListExtensions
    {
        public static Camera FirstByIPAddress(this IEnumerable<Camera> cameras, string ipAddress) => cameras.First(x => x.IPAddress == ipAddress); 
    }
}
