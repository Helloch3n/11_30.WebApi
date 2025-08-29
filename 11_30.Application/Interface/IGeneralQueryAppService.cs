using _11_30.Application.Dtos;
using _11_30.Domain.Entities.GeneralQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _11_30.Application.Interface
{
    public interface IGeneralQueryAppService
    {
        public Task<List<GeneralQueryTypeDto>> GetAllTypeAsync();
        public Task<List<GeneralQueryActionDto>> GetActionByTypeIdAsync(Guid typeId);
        public Task AddAction(GeneralQueryActionDto generalQueryActionDto);
        public Task<List<GeneralQueryFieldDto>> GetFieldByActionIdAsync(Guid actionId);
        public Task AddField(GeneralQueryFieldDto generalQueryFieldDto);
        public Task<List<Dictionary<string, object>>> ErpQueryAsync(string columns, string table, string orderby, string filter);
        public Task<MemoryStream> ExportToExcelAsync(List<Dictionary<string, object>> rows);
    }
}
