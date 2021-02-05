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
        public async Task<IActionResult> GetList([FromQuery] InboxListRequest request)
        {
            return Ok(await _service.GetListEmails(request));
        }

        [HttpGet("show")]
        public async Task<IActionResult> GetMail([FromQuery] InboxMailRequest request)
        {
            return Ok(await _service.GetEmailMessage(request));
        }
    }
}
