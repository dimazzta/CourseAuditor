using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseAuditor.Models
{
    public class Group
    {
        public int Id { get; set; }
        [Column("Course_ID")]
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public int Number { get; set; }
    }
}
