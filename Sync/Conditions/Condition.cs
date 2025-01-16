using System.Collections.Generic;

namespace Sync
{
    public class Condition
        {
            public string Parameter { get; set; }
            public string Operator { get; set; }
            public string Value { get; set; }
            public string SecondaryValue { get; set; }
            public List<string> Values { get; set; }
        }

}
