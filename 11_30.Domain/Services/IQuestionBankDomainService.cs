using _11_30.Domain.Entities.QuestionBank;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _11_30.Domain.Services
{
    public interface IQuestionBankDomainService
    {
        /// <summary>
        /// 根据传入的选项列表，判断哪些选项是正确答案
        /// </summary>
        /// <param name="question">问题</param>
        /// <param name="options">选项</param>
        /// <returns>正确选项列表</returns>
        public List<int> GetTrueOptionsService(Question question, List<string> options);

    }
}
