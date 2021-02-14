using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using YogoServer.Requests;
using YogoServer.Responses;
using YogoServer.Services;

namespace YogoServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.FailedDependency)]
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
        [Produces(typeof(Emails))]
        public async Task<IActionResult> List([FromQuery] InboxListRequest request)
        {
            return Ok(await _service.Get(request, nameof(List).ToLower()));
        }

        [HttpGet("show")]
        [Produces(typeof(Email))]
        public async Task<IActionResult> Show([FromQuery] InboxMailRequest request)
        {
            return Ok(await _service.Get(request, nameof(Show).ToLower()));
        }
    }
}
