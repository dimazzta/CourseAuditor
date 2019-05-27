using System.Collections;

namespace CourseAuditor.Models
{
    class Parent
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Patronymic { get; set; }
        public string Phone { get; set; }

        public virtual ICollection<PersonPerent> Children { get; set; }
    }
}
