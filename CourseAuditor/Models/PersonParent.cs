using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseAuditor.Models
{
    public class PersonParent : ObservableObject
    {
        [ForeignKey("Person")]
        public int Person_ID { get; set; }
        [ForeignKey("Parent")]
        public int Parent_ID { get; set; }


        private Person _Person;
        private Parent _Parent;

        public virtual Person Person
        {
            get
            {
                return _Person;
            }
            set
            {
                _Person = value;
                OnPropertyChanged("Person");
            }
        }

        public virtual Parent Parent
        {
            get
            {
                return _Parent;
            }
            set
            {
                _Parent = value;
                OnPropertyChanged("Parent");
            }
        }
    }
}
