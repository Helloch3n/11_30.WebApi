using ActiveDirectoryService.Application.Dtos;
using ActiveDirectoryService.Application.Events;
using ActiveDirectoryService.Application.Interface;
using ActiveDirectoryService.Infrastructure.ActiveDirectory;
using EventBus.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace ActiveDirectoryService.Application.Services
{
    public class PasswordService : IPasswordService
    {
        private readonly IActiveDirectoryClient _activeDirectoryClient;
        private readonly IEventBus _eventBus;
        private readonly IConfiguration _configuration;

        public PasswordService(IActiveDirectoryClient activeDirectoryClient, IEventBus eventBus, IConfiguration configuration)
        {
            _activeDirectoryClient=activeDirectoryClient;
            _eventBus=eventBus;
            _configuration=configuration;
        }

        public bool ChangePasswordAsync(ChangePasswordRequestDto dto)
        {
            //修改密码
            return _activeDirectoryClient.ChangePasswordAsync(dto.UserName, dto.OldPassword, dto.NewPassword);
        }

        public async Task<bool> ChangePasswordByCapchaAsync(ValidateRequestDto dto)
        {
            //修改密码
            return await _activeDirectoryClient.ChangePasswordByCapchaAsync(dto.UserName, dto.NewPassword, dto.Capcha);
        }

        public async Task SendCapcha(ForgotPasswordRequestDto dto)
        {
            string sysCode = _configuration["SysCode"] ?? string.Empty;
            var capcha = await _activeDirectoryClient.SendCapchaAsync(dto.UserName);
            var msg = $"AD域验证码：{capcha}";
            var forgotPasswordEvents = new ForgotPasswordRequestedEvent(dto.UserName, msg, sysCode);
            _eventBus.Publish(forgotPasswordEvents);
        }
    }
}
