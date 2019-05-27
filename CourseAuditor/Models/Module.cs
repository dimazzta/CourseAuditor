using System;

namespace CourseAuditor.Models
{
   public class Module
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public  Group Group   { get; set; }
        public int Number { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }


}
}