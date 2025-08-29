using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiveDirectoryService.Application.Dtos
{
    public record ValidateRequestDto(string UserName, string NewPassword, string Capcha);
}
    