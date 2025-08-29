using _11_30.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _11_30.Application.Interface
{
    public interface IQuestionBankAppService
    {
        public Task<List<QuestionBankDto>> GetAllAsync();
    }
}
