using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _11_30.Domain.Entities.Questions
{
    public enum QuestionType
    {
        [Description("单选题")]
        SingleChoice = 1,

        [Description("多选题")]
        MultipleChoice = 2,

        [Description("判断题")]
        TrueFalse = 3,
    }
}
