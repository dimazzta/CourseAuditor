using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseAuditor.Models
{

    public class MedicalDoc : ObservableObject
    {
        [ForeignKey("Person")]
        public int Person_ID { get; set; }
        private Person _Person;
        private DateTime _DateStart;
        private DateTime _DateEnd;
        private string _Comment;

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
        public DateTime DateEnd
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

        public string Comment
        {
            get
            {
                return _Comment;
            }
            set
            {
                _Comment = value;
                OnPropertyChanged("Comment");
            }
        }

    }
}
