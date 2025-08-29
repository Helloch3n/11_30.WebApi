using _11_30.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _11_30.Domain.Entities.GeneralQuery
{
    public class GeneralQueryField(string name, string description, Guid generalQueryActionId) : Entity<Guid>
    {
        public string Name { get; private set; } = name;
        public string Description { get; private set; } = description;
        public Guid GeneralQueryActionId { get; private set; } = generalQueryActionId;
    }
}
