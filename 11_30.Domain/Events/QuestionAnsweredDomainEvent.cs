using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _11_30.Domain.Event
{
    public class QuestionAnsweredDomainEvent : IDomainEvent
    {
        public Guid QuestionId { get; }
        public DateTime OccurredOn { get; } = DateTime.Now;
        public QuestionAnsweredDomainEvent(Guid questionId)
        {
            QuestionId = questionId;
        }
    }
}
