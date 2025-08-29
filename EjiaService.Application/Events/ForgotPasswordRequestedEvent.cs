using EventBus.Abstractions;

namespace EjiaService.Application.Events
{
    public class ForgotPasswordRequestedEvent : IntegrationEvent
    {
        public string UserName { get; }
        public string Capcha { get; }
        public string SysCode { get; }

        public DateTime RequestedAt { get; }

        public ForgotPasswordRequestedEvent(string userName, string capcha)
        {
            UserName = userName;
            Capcha = capcha;
            RequestedAt = DateTime.Now;
        }
    }
}
