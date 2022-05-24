using Microsoft.AspNetCore.Mvc;

namespace ActionDetection.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class CameraController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public CameraController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IEnumerable<string> List()
        {
            return _configuration.GetSection("Cameras").GetChildren().Select(x => x.Value);
        }
    }
} 