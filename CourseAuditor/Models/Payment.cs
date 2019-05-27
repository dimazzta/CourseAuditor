using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseAuditor.Models
{
    public class Payment
    {
        public int Id { get; set; }
        [Column("Student_ID")]
        public int StudentId { get; set; }
        public virtual Student Student { get; set; }
        public double Sum { get; set; }
        public DateTime Date { get; set; }
        public double? Discount { get; set; }
    }
}
