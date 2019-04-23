using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseAuditor.Models
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(): base("default")
        {

        }
        public DbSet<User> Users { get; set; }
    }
}
