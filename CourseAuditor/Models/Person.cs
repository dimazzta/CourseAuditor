using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseAuditor.Models
{
    public class Person
    {
        public int Id { get; set; }
        [Column("First_Name")]
        public string FirstName { get; set; }
        [Column("Second_Name")]
        public string SecondName { get; set; }
        public string Patronymic { get; set; }

        public virtual ICollection<PersonParent> Parents { get; set; }
    }
}
