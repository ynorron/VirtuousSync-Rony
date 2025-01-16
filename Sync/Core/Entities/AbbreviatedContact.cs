using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sync
{
    public class AbbreviatedContact
    {
        public int Id { get; set; }
        public int ExternalId { get; set; }
        public string Name { get; set; }

        public string ContactType { get; set; }

        public string ContactName { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }
    }
}
