using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseAuditor.Models
{
    public class Module
    {
        public int Id { get; set; }
        [Column("Group_ID")]
        public int GroupId { get; set; }
        public Group Group { get; set; }
        public int Number { get; set; }
        [Column("Date_Start")]
        public DateTime DateStart { get; set; }
        [Column("Date_End")]
        public DateTime DateEnd { get; set; }


}
}