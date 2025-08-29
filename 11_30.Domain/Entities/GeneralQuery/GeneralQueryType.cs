using _11_30.Domain.Entities.Common;
using _11_30.Domain.Entities.QuestionBank;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _11_30.Domain.Entities.GeneralQuery
{
    public class GeneralQueryType(string name, string url) : Entity<Guid>
    {
        public string Name { get; private set; } = name;
        public string Url { get; private set; } = url;
        public string RequestContent { get; private set; } = string.Empty;

        private readonly List<GeneralQueryAction> _actions = [];
        public IReadOnlyCollection<GeneralQueryAction> Actions => _actions;


        public void AddAction(string name, string description)
        {
            var action = new GeneralQueryAction(name, description, this.Id);
            _actions.Add(action);
        }

        public void AddFieldToActions(Guid actionId, string name, string description)
        {
            var field = new GeneralQueryField(name, description, Id);
            var action = _actions.FirstOrDefault(o => o.Id==actionId);
            action.AddField(name, description);
        }
    }
}
