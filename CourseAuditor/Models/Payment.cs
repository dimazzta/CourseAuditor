using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseAuditor.Models
{
    public enum PaymentType
    {
        OneTime = 0,
        Month = 1,
        Module = 2,
        Arbitraty = 3
    };

    public class Payment : ObservableObject
    {
        [ForeignKey("Student")]
        public int Student_ID { get; set; }

        private Student _Student;
        private double _Sum;
        private DateTime _Date;
        private double? _Discount;
        private PaymentType _Type;

        public override string ToString()
        {
            return $"{Date.ToString("dd MMM yyyy")} : {Sum}р. (Скидка {Discount * 100}%)";
        }

        public PaymentType Type
        {
            get
            {
                return _Type;
            }
            set
            {
                _Type = value;
                OnPropertyChanged("Type");
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
        public double Discount
        {
            get
            {
                return _Discount ?? 0;
            }
            set
            {
                _Discount = value;
                OnPropertyChanged("Discount");
            }
        }
    }
}
