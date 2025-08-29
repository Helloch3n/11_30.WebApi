using _11_30.Domain.Entities.QuestionBank;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _11_30.Domain.Services
{
    public class QuestionBankDomainService : IQuestionBankDomainService
    {
        public List<int> GetTrueOptionsService(Question question, List<string> options)
        {
            try
            {
                List<int> result = [];
                string answer = question.Answers;
                List<string> answers = [.. answer.Split(",")];
                foreach (var ans in answers)
                {
                    var propertyName = $"Option{ans}";
                    string trueAnswer = typeof(Question).GetProperty(propertyName)?.GetValue(question).ToString();
                    foreach (var option in options)
                    {
                        if (option == trueAnswer)
                        {
                            var index = options.IndexOf(option);
                            result.Add(index);
                        }
                    }
                }
                if (result.Count==0)
                {
                    result.Add(1);
                }
                return result;
            }
            catch (Exception ex)
            {
                return [0];
            }
        }
    }
}
