using _11_30.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _11_30.Domain.Entities.GeneralQuery
{
    public class GeneralQueryAction(string name, string description, Guid generalQueryTypeId) : Entity<Guid>
    {
        public string Name { get; private set; } = name;
        public string Description { get; private set; } = description;
        public Guid GeneralQueryTypeId { get; set; } = generalQueryTypeId;
        private readonly List<GeneralQueryField> _fields = [];
        public IReadOnlyCollection<GeneralQueryField> Fields => _fields;




        public void AddField(string name, string description)
        {
            var field = new GeneralQueryField(name, description, this.Id);
            _fields.Add(field);
        }
    }
}
