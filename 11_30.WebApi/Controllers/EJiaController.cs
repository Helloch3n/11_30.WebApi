using _11_30.Application.Dtos;
using _11_30.Application.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace _11_30.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EJiaController : ControllerBase
    {
        private readonly IEJiaAppService _ejiAppService;

        public EJiaController(IEJiaAppService ejiAppService)
        {
            _ejiAppService=ejiAppService;
        }

        [HttpPost()]
        public async Task<ApiResponse> EJiaAutoNewsAsync(EJiaDto eJiaDto)
        {
            var passWrod = eJiaDto.PassWord;
            var userName = eJiaDto.UserName;
            var startDate = eJiaDto.StartDate;
            var endDate = eJiaDto.EndDate;
            string newsType = eJiaDto.NewsType;
            try
            {
                await _ejiAppService.AutoNewsAsync(userName, passWrod, startDate, endDate, newsType);
                return ApiResponse.Success();
            }
            catch (Exception ex)
            {
                return ApiResponse.Fail(ex.ToString());
            }

        }
    }
}
