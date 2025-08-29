using _11_30.Application.Dtos;
using _11_30.Application.Interface;
using Microsoft.AspNetCore.Mvc;

namespace _11_30.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GeneralQueryOptionController : ControllerBase
    {
        private readonly IGeneralQueryAppService _queryTypeAppService;


        public GeneralQueryOptionController(IGeneralQueryAppService queryTypeAppService)
        {
            _queryTypeAppService=queryTypeAppService;
        }

        [HttpGet]
        public async Task<List<GeneralQueryTypeDto>> GetGeneralQueryTypesAsync()
        {
            return await _queryTypeAppService.GetAllTypeAsync();
        }

        [HttpGet]
        public async Task<List<GeneralQueryFieldDto>> GetGeneralQueryFieldsAsync(Guid actionId)
        {
            return await _queryTypeAppService.GetFieldByActionIdAsync(actionId);
        }

        [HttpGet]
        public async Task<List<GeneralQueryActionDto>> GetGeneralQueryActionsAsync(Guid typeId)
        {
            return await _queryTypeAppService.GetActionByTypeIdAsync(typeId);
        }

        [HttpPost]
        public async Task AddGeneralQueryActionAsync(GeneralQueryActionDto generalQueryActionDto)
        {
            await _queryTypeAppService.AddAction(generalQueryActionDto);
        }

        [HttpPost]
        public async Task AddGeneralFieldActionAsync(GeneralQueryFieldDto generalQueryFieldDto)
        {
            await _queryTypeAppService.AddField(generalQueryFieldDto);
        }
    }
}
