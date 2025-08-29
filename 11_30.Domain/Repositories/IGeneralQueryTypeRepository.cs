using _11_30.Domain.Entities.GeneralQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _11_30.Domain.Repositories
{
    public interface IGeneralQueryTypeRepository
    {
        public Task<GeneralQueryType> GetByIdAsync(Guid typeId);
        public Task AddAsync(GeneralQueryType generalQueryType);
        public Task<List<GeneralQueryType>> GetAllAsync();
        public Task<GeneralQueryAction> GetActionByIdAsync(Guid Id);
        public Task<List<GeneralQueryAction>> GetAllActionsAsync();
        public Task<List<GeneralQueryAction>> GetActionsByTypeIdAsync(Guid typeId);
        public Task<List<GeneralQueryField>> GetAllFieldsAsync();
        public Task<List<GeneralQueryField>> GetFieldsByActionIdAsync(Guid actionId);
    }
}
