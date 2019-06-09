using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseAuditor.Models
{
    public class Student : ObservableObject
    {
        private Person _Person;
        private DateTime _DateStart;
        private DateTime? _DateEnd;
        private Group _Group;
        private ICollection<Journal> _Journals;
        private ICollection<Return> _Returns;
        private ICollection<Payment> _Payments;
        

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

        public DateTime DateStart
        {
            get
            {
                return _DateStart;
            }
            set
            {
                _DateStart = value;
                OnPropertyChanged("DateStart");
            }
        }

        public DateTime? DateEnd
        {
            get
            {
                return _DateEnd;
            }
            set
            {
                _DateEnd = value;
                OnPropertyChanged("DateEnd");
            }
        }

        public virtual Group Group
        {
            get
            {
                return _Group;
            }
            set
            {
                _Group = value;
                OnPropertyChanged("Group");
            }
        }

        public virtual ICollection<Journal> Journals
        {
            get
            {
                return _Journals;
            }
            set
            {
                _Journals = value;
                OnPropertyChanged("Journals");
            }
        }

        public virtual ICollection<Return> Returns
        {
            get
            {
                return _Returns;
            }
            set
            {
                _Returns = value;
                OnPropertyChanged("Returns");
            }
        }

        public virtual ICollection<Payment> Payments
        {
            get
            {
                return _Payments;
            }
            set
            {
                _Payments = value;
                OnPropertyChanged("Payments");
            }
        }

        public override string ToString()
        {
            return Person.FullName;
        }
    }
}
