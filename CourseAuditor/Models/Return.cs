using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseAuditor.Models
{
    public class Return : ObservableObject
    {
        [ForeignKey("Student")]
        public int Student_ID { get; set; }

        private Student _Student;
        private DateTime _Date;
        private double _Sum;

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
        public double Sum
        {
            get
            {
                return _Sum;
            }
            set
            {
                _Sum = value;
                OnPropertyChanged("Sum");
            }
        }
    }
}
