using EjiaService.Application.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EjiaService.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class yunzhijiaController : ControllerBase
    {
        private readonly ILogger<yunzhijiaController> _logger;
        private readonly IEjiaMessagePush _ejiaMessagePush;


        public yunzhijiaController(ILogger<yunzhijiaController> logger, IEjiaMessagePush ejiaMessagePush)
        {
            _logger = logger;
            _ejiaMessagePush=ejiaMessagePush;
        }

        [HttpPost]
        [HttpGet]
        public async Task<IActionResult> Post([FromQuery] string sysCode, [FromQuery] string userCode, [FromQuery] string message)
        {
            if (string.IsNullOrWhiteSpace(sysCode))
            {
                return BadRequest(new { code = "E", message = "系统编码不能为空" });
            }
            if (string.IsNullOrWhiteSpace(userCode))
            {
                return BadRequest(new { code = "E", message = "员工工号不能为空" });
            }
            if (string.IsNullOrWhiteSpace(message))
            {
                return BadRequest(new { code = "E", message = "信息内容不能为空" });
            }

            try
            {
                var response = await _ejiaMessagePush.EJiaMessagePush(sysCode, userCode, message);
                if (response == "OK")
                {
                    return Ok(new { code = "S", message = "推送成功" });
                }
                return BadRequest(new { code = "E", message = "推送失败" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { code = "E", message = ex });
            }
        }
    }
}
