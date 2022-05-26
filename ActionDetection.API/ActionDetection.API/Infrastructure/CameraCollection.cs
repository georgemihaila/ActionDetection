using System.Collections;

namespace ActionDetection.API.Infrastructure
{
    public class CameraCollection : IEnumerable<Camera>
    {
        private readonly IEnumerable<Camera> _cameras;

        public CameraCollection(IEnumerable<Camera> cameras)
        {
            _cameras = cameras;
            Console.WriteLine("Reinit CameraCollection " + DateTime.Now.Ticks);
        }

        public IEnumerator<Camera> GetEnumerator()
        {
            return _cameras.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_cameras).GetEnumerator();
        }
    }
}
