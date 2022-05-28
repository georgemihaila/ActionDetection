using ActionDetection.API.Infrastructure;
using ActionDetection.API.Infrastructure.ObjectDetection;
using ActionDetection.API.Middlewares;

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

app.UseAuthorization();

app.MapControllers();

app.Run();   