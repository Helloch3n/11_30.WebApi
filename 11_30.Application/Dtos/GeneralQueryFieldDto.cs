using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _11_30.Application.Dtos
{
    public class GeneralQueryFieldDto
    {
        public Guid id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid GeneralQueryActionId { get; set; }
    }
}
