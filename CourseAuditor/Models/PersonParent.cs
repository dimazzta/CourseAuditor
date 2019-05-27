using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseAuditor.Models
{
    [Table("Person_Parent")]
    public class PersonParent
    {
        public int Id { get; set; }
        [Column("Person_ID")]
        public int PersonId { get; set; }
        public virtual Person Person { get; set; }
        [Column("Parent_ID")]
        public int ParentId { get; set; }
        public virtual Parent Parent { get; set; }
    }
}
