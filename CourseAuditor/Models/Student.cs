using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseAuditor.Models
{
    public class Student
    {
        public int Id { get; set; }
        [Column("Person_ID")]
        public int PersonId { get; set; }
        public virtual Person Person { get; set; }
        [Column("Date_Start")]
        public DateTime DateStart { get; set; }
        [Column("Date_End")]
        public DateTime? DateEnd { get; set; }
        [Column("Group_ID")]
        public int GroupId { get; set; }
        public virtual Group Group { get; set; }
    }
}
