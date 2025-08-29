using _11_30.Domain.Entities.Common;
using _11_30.Domain.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _11_30.Domain.Entities.QuestionBank
{
    public class QuestionBank(string name, string xPathEndStr) : Entity<Guid>
    {
        public string Name { get; private set; } = name;
        public string XPathEndStr { get; private set; } = xPathEndStr;

        private readonly List<Question> _questions = [];
        public IReadOnlyCollection<Question> Questions => _questions;
        public bool IsOnGoing { get; private set; }

        //添加问题
        public void AddQuestion(string title)
        {
            _questions.Add(new Question(title, this.Id));
        }

        //添加事件
        public void AddAnsweredEvent()
        {
            AddDomainEvent(new QuestionAnsweredDomainEvent(this.Id));
        }
    }
}
