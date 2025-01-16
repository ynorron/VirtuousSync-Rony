using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sync.QueryBuilders
{
    internal class GroupBuilder
    {
        private readonly List<Group> _groups = new List<Group>();

        public GroupBuilder AddGroup(params Condition[] conditions)
        {
            var group = new Group();
            group.Conditions.AddRange(conditions);
            _groups.Add(group);
            return this;
        }

        public List<Group> Build() => _groups;
    }
}
