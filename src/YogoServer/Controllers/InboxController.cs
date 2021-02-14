using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using YogoServer.Requests;
using YogoServer.Services;

namespace YogoServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InboxController : ControllerBase
    {
        private readonly ILogger<InboxController> _logger;
        private readonly IYogoService _service;

        public InboxController(ILogger<InboxController> logger, IYogoService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet("list")]
        public async Task<IActionResult> List([FromQuery] InboxListRequest request)
        {
            return Ok(await _service.Get(request, nameof(List).ToLower()));
        }

        [HttpGet("show")]
        public async Task<IActionResult> Show([FromQuery] InboxMailRequest request)
        {
            return Ok(await _service.Get(request, nameof(Show).ToLower()));
        }
    }
}
