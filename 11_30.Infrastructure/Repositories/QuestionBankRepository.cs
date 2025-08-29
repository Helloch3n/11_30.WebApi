using _11_30.Domain.Entities.QuestionBank;
using _11_30.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _11_30.Infrastructure.Repositories
{
    public class QuestionBankRepository : IQuestionBankRepository
    {
        private readonly AppDbContext _appDbContext;

        public QuestionBankRepository(AppDbContext appDbContext)
        {
            _appDbContext=appDbContext;
        }

        public async Task<List<QuestionBank>> GetAllAsync()
        {
            List<QuestionBank> dbQuestionBanks = await _appDbContext.QuestionBanks.ToListAsync();
            return dbQuestionBanks;
        }

        public async Task<Question> GetQuestionByIdAsync(Guid id)
        {
            var dbQuestion = await _appDbContext.Questions.Include(o => o.QuestionType).FirstOrDefaultAsync(o => o.Id == id);
            return dbQuestion;
        }

        public async Task<Question> GetQuestionByTitleAsync(string title)
        {
            title=title.Replace('（', '%');
            title=title.Replace('）', '%');
            title=title.Replace(' ', '%');
            Question dbQuestion = await _appDbContext.Questions.Where(o => EF.Functions.Like(o.Title, $"%{title}%")).FirstOrDefaultAsync();
            return dbQuestion;
        }

        public async Task<QuestionBank> GetByIdAsync(Guid id)
        {
            var dbQuestionBank = await _appDbContext.QuestionBanks.Where(o => o.Id==id).FirstOrDefaultAsync();
            return dbQuestionBank;
        }
    }
}
