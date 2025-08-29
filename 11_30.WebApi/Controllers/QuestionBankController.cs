using _11_30.Application.Dtos;
using _11_30.Application.Interface;
using Microsoft.AspNetCore.Mvc;

namespace _11_30.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class QuestionBankController : ControllerBase
    {
        private readonly IQuestionBankAppService _questionBankAppService;

        public QuestionBankController(IQuestionBankAppService questionBankAppService)
        {
            _questionBankAppService=questionBankAppService;
        }

        [HttpGet]
        public async Task<List<QuestionBankDto>> GetQuestionBanks()
        {
            var questionDto = await _questionBankAppService.GetAllAsync();
            return questionDto;
        }
    }
}
