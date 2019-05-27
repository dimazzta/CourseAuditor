using System.Collections;
using System.Collections.Generic;

namespace CourseAuditor.Models
{
    public class Parent
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Patronymic { get; set; }
        public string Phone { get; set; }

        public virtual ICollection<PersonParent> Children { get; set; }
    }
}
