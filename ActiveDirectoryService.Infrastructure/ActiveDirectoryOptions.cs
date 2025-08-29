using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiveDirectoryService.Infrastructure
{
    public class ActiveDirectoryOptions
    {
        public string Domain { get; set; } = default!;
        public string DomainIp { get; set; } = default!;
        public string AdminUsername { get; set; } = default!;
        public string AdminPassword { get; set; } = default!;
    }
}
