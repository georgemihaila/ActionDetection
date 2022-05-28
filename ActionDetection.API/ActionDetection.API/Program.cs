using ActionDetection.API.Infrastructure;
using ActionDetection.API.Infrastructure.BackgroundTasks;
using ActionDetection.API.Infrastructure.ObjectDetection;
using ActionDetection.API.Middlewares;

using Hangfire;
using Hangfire.MemoryStorage;

using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ObjectDetectionService>();
var cameras = builder.Configuration.GetSection("Cameras").GetChildren().Select(x => new Camera(x.Value)).ToArray();
builder.Services.AddSingleton(new CameraCollection(cameras));
builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.AllowSynchronousIO = true;
});
builder.Services.Configure<IISServerOptions>(options =>
{
    options.AllowSynchronousIO = true;
});

var inMemoryData = new Hangfire.MemoryStorage.Database.Data();
var memoryOptions = new MemoryStorageOptions();
JobStorage.Current = new MemoryStorage(memoryOptions, inMemoryData);
builder.Services.AddHangfire(x => x.UseMemoryStorage(memoryOptions, inMemoryData));
builder.Services.AddHangfireServer();
builder.Services.AddScoped<UpdateCameraFramesPeriodicallyTask>();
RecurringJob.AddOrUpdate<UpdateCameraFramesPeriodicallyTask>("UpdateCameraFramesPeriodicallyTask", x => x.RunAsync(), CronConstants.Every5s);
var app = builder.Build();
app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true) // allow any origin
                .AllowCredentials()); // allow credentials
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<AccesStatsMiddleware>();
app.UseWebSockets();
app.UseHangfireDashboard();
app.UseAuthorization();

app.MapControllers();

app.Run();