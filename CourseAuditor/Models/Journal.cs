using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseAuditor.Models
{
    public class Journal : ObservableObject
    {

        private Student _Student;
        private DateTime _Date;
        private Assessment _Assessment;


        public virtual Assessment Assessment
        {
            get
            {
                return _Assessment;
            }
            set
            {
                _Assessment = value;
                OnPropertyChanged("Assessment");
            }
        }
        public virtual Student Student
        {
            get
            {
                return _Student;
            }
            set
            {
                _Student = value;
                OnPropertyChanged("Student");
            }
        }

        
        public DateTime Date
        {
            get
            {
                return _Date;
            }
            set
            {
                _Date = value;
                OnPropertyChanged("Date");
            }
        }

       
    }
}
