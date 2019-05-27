using System.ComponentModel.DataAnnotations.Schema;

namespace CourseAuditor.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        [Column("Lessons_Count")]
        public int LessonsCount { get; set; }
    }
}
