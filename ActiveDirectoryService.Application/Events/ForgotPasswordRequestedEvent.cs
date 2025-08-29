using EventBus.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiveDirectoryService.Application.Events
{
    public class ForgotPasswordRequestedEvent : IntegrationEvent
    {
        public string UserName { get; }
        public string Capcha { get; }

        public string SysCode { get; }

        public DateTime RequestedAt { get; }

        public ForgotPasswordRequestedEvent(string userName, string capcha, string sysCode)
        {
            UserName = userName;
            Capcha = capcha;
            SysCode = sysCode;
            RequestedAt = DateTime.Now;
        }
    }
}
