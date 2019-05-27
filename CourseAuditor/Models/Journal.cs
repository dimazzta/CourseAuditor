using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseAuditor.Models
{
    public class Journal
    {
        public int Id { get; set; }
        [Column("Student_ID")]
        public int StudentId { get; set; }
        public virtual Student Student { get; set; }

        public DateTime Date { get; set; }
        [Column("Attendance_assessment")]
        public string AttendanceAssessment { get; set; }
    }
}
