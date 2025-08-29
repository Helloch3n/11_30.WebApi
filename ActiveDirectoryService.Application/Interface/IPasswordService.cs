using ActiveDirectoryService.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiveDirectoryService.Application.Interface
{
    public interface IPasswordService
    {
        bool ChangePasswordAsync(ChangePasswordRequestDto dto);

        Task SendCapcha(ForgotPasswordRequestDto dto);

        Task<bool> ChangePasswordByCapchaAsync(ValidateRequestDto dto);
    }
}
