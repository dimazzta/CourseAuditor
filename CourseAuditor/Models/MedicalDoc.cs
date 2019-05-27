using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseAuditor.Models
{
    [Table("Medical_Doc")]
    public class MedicalDoc
    {
        public int Id { get; set; }
        [Column("Person_ID")]
        public int PersonId { get; set; }
        public virtual Person Person { get; set; }
        [Column("Date_Start")]
        public DateTime DateStart { get; set; }
        [Column("Date_End")]
        public DateTime DateEnd { get; set; }
        public string Comment { get; set; }

    }
}
