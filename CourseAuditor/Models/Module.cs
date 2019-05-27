using System;

namespace CourseAuditor.Models
{
    class Module
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public int Group Group  { get; set; }
        public int Number { get; set; }
        public DateTime DateStart { get; set }
        public DateTime DateEnd { get; set }


}
}