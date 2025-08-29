using _11_30.Application.Dtos;
using _11_30.Application.Interface;
using _11_30.Domain.Entities.QuestionBank;
using _11_30.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _11_30.Application.Services
{
    public class QuestionBankAppService : IQuestionBankAppService
    {
        private readonly IQuestionBankRepository _questionBankRepository;

        public QuestionBankAppService(IQuestionBankRepository questionBankRepository)
        {
            _questionBankRepository=questionBankRepository;
        }
        public async Task<List<QuestionBankDto>> GetAllAsync()
        {
            var dbQuestionBanks = await _questionBankRepository.GetAllAsync();
            var dtos = dbQuestionBanks.Select(qb => qb.ToDto()).ToList();
            return dtos;
        }
    }
}
