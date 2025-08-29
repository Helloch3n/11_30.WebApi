using _11_30.Domain.Entities.Common;
using _11_30.Domain.Entities.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _11_30.Domain.Entities.QuestionBank
{
    public class Question(string title, Guid questionBankId) : Entity<Guid>
    {
        public Guid QuestionBankId { get; private set; } = questionBankId;
        public string Title { get; private set; } = title;
        public QuestionType QuestionType { get; private set; }
        public string Answers { get; private set; }
        public string OptionA { get; private set; }
        public string OptionB { get; private set; }
        public string OptionC { get; private set; }
        public string OptionD { get; private set; }
        public string OptionE { get; private set; }
        public string OptionF { get; private set; }
    }
}
