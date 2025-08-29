using _11_30.Application.Dtos;
using _11_30.Application.Interface;
using _11_30.Infrastructure.External.SOA;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace _11_30.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GeneralQueryController : ControllerBase
    {
        private readonly IGeneralQueryAppService _gqAppService;

        public GeneralQueryController(IGeneralQueryAppService gqAppService)
        {
            _gqAppService=gqAppService;
        }

        [HttpPost]
        public async Task<ApiResponse<List<Dictionary<string, object>>>> ErpQueryAsync(ApiResponse<ErpQueryDto> apiResponse)
        {
            string columns = apiResponse.Data.Columns;
            string table = apiResponse.Data.Table;
            string orderby = apiResponse.Data.Orderby;
            string filter = apiResponse.Data.Filter;
            //调用ERP查询
            var dataList = await _gqAppService.ErpQueryAsync(columns, table, orderby, filter);
            //返回结果
            var response = new ApiResponse<List<Dictionary<string, object>>>() { Code=200, Message="", Data=dataList };
            return response;
        }

        [HttpPost]
        public async Task<IActionResult> ExportToExcelAsync([FromBody] List<Dictionary<string, object>> rows)
        {
            var stream = await _gqAppService.ExportToExcelAsync(rows);
            var fileName = $"导出数据_ERP通用查询_{DateTime.Now:yyyyMMdd}.xlsx";
            Response.Headers.Add("Content-Disposition", $"attachment; filename*=UTF-8''{Uri.EscapeDataString(fileName)}");
            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

    }
}
