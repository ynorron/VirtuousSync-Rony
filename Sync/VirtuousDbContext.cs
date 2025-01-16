using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sync
{
    internal class VirtuousDbContext : DbContext
    {
        public VirtuousDbContext() : base("name=DefaultConnection") // Matches the name in App.config
        {
        }

        public DbSet<AbbreviatedContact> Contacts { get; set; }
    }

}
