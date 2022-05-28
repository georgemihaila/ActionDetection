namespace ActionDetection.API.Infrastructure.BackgroundTasks
{
    public interface IHangfireBackgroundService
    {
        public Task RunAsync();
    }
}
