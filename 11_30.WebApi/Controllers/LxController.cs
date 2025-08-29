using _11_30.Application.Interface;
using Microsoft.AspNetCore.Mvc;

namespace _11_30.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LxController : ControllerBase
    {
        private readonly ILxAppService _lxAppService;

        public LxController(ILxAppService lxAppService)
        {
            _lxAppService = lxAppService;
        }

        [HttpGet()]
        public async Task<IActionResult> AutoAnswersAsync()
        {
            await _lxAppService.LxAutoAnswerAsync();
            return Ok();
        }
    }
}
