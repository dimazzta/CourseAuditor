using System;

namespace CourseAuditor.Models
{
    public class Journal
    {
        public int Id { get; set; }

        public int StudentId { get; set; }
        public virtual Student Student { get; set; }

        public DateTime Date { get; set; }
        public string Attendance_assessment { get; set; }
    }
}
