using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseAuditor.Models
{
    public class Parent
    {
        public int Id { get; set; }
        [Column("First_Name")]
        public string FirstName { get; set; }
        [Column("Second_Name")]
        public string SecondName { get; set; }
        public string Patronymic { get; set; }
        public string Phone { get; set; }

        public virtual ICollection<PersonParent> Children { get; set; }
    }
}
