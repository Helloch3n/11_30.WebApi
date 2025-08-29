using _11_30.Application.Dtos;
using _11_30.Application.Interface;
using _11_30.Domain;
using _11_30.Domain.Entities.GeneralQuery;
using _11_30.Domain.Repositories;
using _11_30.Infrastructure.External.Excel;
using _11_30.Infrastructure.External.SOA;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _11_30.Application.Services
{
    public class GeneralQueryAppService : IGeneralQueryAppService
    {
        private readonly IGeneralQueryTypeRepository _queryTypeRep;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IErpService _erpService;
        private readonly IExcelService _excelService;

        public GeneralQueryAppService(IGeneralQueryTypeRepository generalQueryTypeRepository, IUnitOfWork unitOfWork, IErpService erpService, IExcelService excelService)
        {
            _queryTypeRep=generalQueryTypeRepository;
            _unitOfWork=unitOfWork;
            _erpService=erpService;
            _excelService=excelService;
        }

        public async Task AddAction(GeneralQueryActionDto generalQueryActionDto)
        {
            var dbQueryType = await _queryTypeRep.GetByIdAsync(generalQueryActionDto.GeneralQueryTypeId);
            dbQueryType.AddAction(generalQueryActionDto.Name, generalQueryActionDto.Description);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task AddField(GeneralQueryFieldDto generalQueryFieldDto)
        {
            var dbAction = await _queryTypeRep.GetActionByIdAsync(generalQueryFieldDto.GeneralQueryActionId);
            dbAction.AddField(generalQueryFieldDto.Name, generalQueryFieldDto.Description);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<List<GeneralQueryActionDto>> GetActionByTypeIdAsync(Guid typeId)
        {
            var dbQueryAction = await _queryTypeRep.GetActionsByTypeIdAsync(typeId);
            var dtoQueryAction = dbQueryAction.Select(o => new GeneralQueryActionDto
            {
                Id=o.Id,
                Name=o.Name,
                Description=o.Description,
                GeneralQueryTypeId=o.GeneralQueryTypeId

            }).ToList();
            return dtoQueryAction;
        }

        public async Task<List<GeneralQueryTypeDto>> GetAllTypeAsync()
        {
            var dbQueryTypes = await _queryTypeRep.GetAllAsync();
            var dtoQueryTypes = dbQueryTypes.Select(e => new GeneralQueryTypeDto
            {
                Id=e.Id,
                Name=e.Name,
                Url=e.Url,
            }).ToList();
            return dtoQueryTypes;
        }

        public async Task<List<GeneralQueryFieldDto>> GetFieldByActionIdAsync(Guid actionId)
        {
            var dbQueryField = await _queryTypeRep.GetFieldsByActionIdAsync(actionId);
            var dtoQueryField = dbQueryField.Select(o => new GeneralQueryFieldDto
            {
                id=o.Id,
                Name=o.Name,
                Description=o.Description,
                GeneralQueryActionId=o.GeneralQueryActionId

            }).ToList();
            return dtoQueryField;
        }
        public async Task<List<Dictionary<string, object>>> ErpQueryAsync(string columns, string table, string orderby, string filter)
        {
            //调用ERP中间表
            var result = await _erpService.GetDataFromErpAsync(columns, table, filter, orderby);
            //XML转List<Dictionary<string, object>>
            List<Dictionary<string, object>> dataList = await _erpService.XmlToListDic(result);
            return dataList;
        }

        public Task<MemoryStream> ExportToExcelAsync(List<Dictionary<string, object>> rows)
        {
            var stream = _excelService.ExportToExcel(rows);
            return Task.FromResult(stream);
        }
    }
}
