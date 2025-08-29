using _11_30.Domain.Entities.GeneralQuery;
using _11_30.Domain.Repositories;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.EntityFrameworkCore;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _11_30.Infrastructure.Repositories
{
    public class GeneralQueryTypeRepository : IGeneralQueryTypeRepository
    {
        private readonly AppDbContext _dbContext;

        public GeneralQueryTypeRepository(AppDbContext dbContext)
        {
            _dbContext=dbContext;
        }

        public async Task AddAsync(GeneralQueryType generalQueryType)
        {
            await _dbContext.AddAsync(generalQueryType);
        }

        public async Task<GeneralQueryAction> GetActionByIdAsync(Guid Id)
        {
            return await _dbContext.GeneralQueryActions.Where(o => o.Id==Id).FirstOrDefaultAsync();
        }

        public async Task<List<GeneralQueryAction>> GetActionsByTypeIdAsync(Guid typeId)
        {
            return await _dbContext.GeneralQueryActions.Where(o => o.GeneralQueryTypeId==typeId).ToListAsync();
        }

        public Task<List<GeneralQueryAction>> GetAllActionsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<List<GeneralQueryType>> GetAllAsync()
        {
            return await _dbContext.GeneralQueryTypes.ToListAsync();
        }

        public Task<List<GeneralQueryField>> GetAllFieldsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<GeneralQueryType> GetByIdAsync(Guid id)
        {
            return await _dbContext.GeneralQueryTypes.Where(o => o.Id==id).FirstOrDefaultAsync();
        }

        public async Task<List<GeneralQueryField>> GetFieldsByActionIdAsync(Guid actionId)
        {
            return await _dbContext.GeneralQueryFields.Where(o => o.GeneralQueryActionId==actionId).ToListAsync();
        }
    }
}
