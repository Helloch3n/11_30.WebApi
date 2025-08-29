using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiveDirectoryService.Application.Dtos
{
    public record ChangePasswordRequestDto(string UserName, string OldPassword, string NewPassword);
}
