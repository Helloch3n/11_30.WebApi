using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiveDirectoryService.Infrastructure.ActiveDirectory
{
    public interface IActiveDirectoryClient
    {
        bool ChangePasswordAsync(string userName, string oldPassword, string newPassword);

        Task<bool> ChangePasswordByCapchaAsync(string userName, string newPassword, string capcha);

        Task<string> SendCapchaAsync(string userName);
    }
}
