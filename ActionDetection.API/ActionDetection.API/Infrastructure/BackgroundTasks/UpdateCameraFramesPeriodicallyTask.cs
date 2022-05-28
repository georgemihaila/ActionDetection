using Hangfire;

namespace ActionDetection.API.Infrastructure.BackgroundTasks
{
    public class UpdateCameraFramesPeriodicallyTask : IHangfireBackgroundService
    {
        private readonly CameraCollection _cameraCollection;

        public UpdateCameraFramesPeriodicallyTask(CameraCollection cameraCollection)
        {
            _cameraCollection = cameraCollection;
        }

        [AutomaticRetry(Attempts = 1, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        public async Task RunAsync()
        {
            Console.WriteLine("Run");
            var tasks = _cameraCollection.Select(x => Task.Run(async () =>
            {
               if (DateTime.Now.Subtract(x.CurrentFrameTime) >= TimeSpan.FromSeconds(3))
                {
                    try
                    {
                        await x.GetFrameAsync(ObjectDetection.ImageSize.VGA);
                    }
                    catch
                    {
                        Console.WriteLine($"Couldn't get frame for {x.IPAddress}");
                    }
                }
            }));
            await Task.WhenAll(tasks);
        }
    }
}
