using EjiaService.Application.Events;
using EjiaService.Application.Interface;
using EventBus.Abstractions;
using Microsoft.Extensions.Logging;

namespace EjiaService.Application.Handler
{
    public class ForgotPasswordRequestedEventHandler : IIntegrationEventHandler<ForgotPasswordRequestedEvent>

    {
        private readonly ILogger<ForgotPasswordRequestedEvent> _logger;
        private readonly IEjiaMessagePush _ejiaMessagePush;

        public ForgotPasswordRequestedEventHandler(ILogger<ForgotPasswordRequestedEvent> logger, IEjiaMessagePush ejiaMessagePush)
        {
            _logger = logger;
            _ejiaMessagePush=ejiaMessagePush;
        }
        public Task Handler(ForgotPasswordRequestedEvent @event)
        {
            // 业务处理...
            _ejiaMessagePush.EJiaMessagePush(@event.SysCode, @event.UserName, @event.Capcha);
            return Task.CompletedTask;
        }
    }
}
