using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _11_30.Domain.Event
{
    public interface IDomainEvent : INotification
    {
        DateTime OccurredOn { get; }
    }
}