using CourseAuditor.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using CourseAuditor.Helpers;

namespace CourseAuditor.Models
{
    public class Student : ObservableObject
    {

        [ForeignKey("Person")]
        public int Person_ID { get; set; }

        [ForeignKey("Module")]
        public int Module_ID { get; set; }

        private Person _Person;
        private DateTime _DateStart;
        private DateTime? _DateEnd;
        private Module _Module;
        private ICollection<Journal> _Journals;
        private ICollection<Return> _Returns;
        private ICollection<Payment> _Payments;
        private double _Balance;
        
        public double CalculatePureBalance()
        {
            double balance = 0;
            using(var _context = new ApplicationContext())
            {
                double payments = _context.Students.First(x => x.ID == ID).Payments.Sum(x => x.Sum);
                balance = payments;
            }
            return balance;
        }

        public void RecalculateBalance()
        {
            var pureBalance = CalculatePureBalance();
            using(var _context = new ApplicationContext())
            {
                var module = _context.Students.First(x => x.ID == ID).Module;
                var journals = _context.Students.Include(t => t.Journals.Select(x => x.Assessment)).First(x => x.ID == ID).Journals.OrderBy(x => x.Date);
                List<Payment> payments = _context.Students.First(x => x.ID == ID).Payments.OrderBy(x => x.Date).ToList();
                int currentPaymentIndex = 0;
                Payment lastPaymentSoFar = null;
                double currentBalance = 0;
                if (payments.Count > currentPaymentIndex)
                {
                    lastPaymentSoFar = payments[currentPaymentIndex];
                    currentBalance = lastPaymentSoFar.Sum;
                }
                  
                foreach(var journal in journals)
                {
                    if (lastPaymentSoFar != null)
                    {
                        if (currentBalance <= 0)
                        {
                            currentPaymentIndex++;
                            if (payments.Count > currentPaymentIndex)
                            {
                                lastPaymentSoFar = payments[currentPaymentIndex];
                                currentBalance = lastPaymentSoFar.Sum;
                            }
                        }
                    }
                    
                    double lastDiscount = lastPaymentSoFar != null ? lastPaymentSoFar.Discount : 0;
                    var toDecrease = module.LessonPrice - module.LessonPrice * lastDiscount;
                    if (journal.Assessment.Type == Constants.Attendance)
                    {
                        bool t = false;
                        if (lastPaymentSoFar != null)
                        {
                            if ((lastPaymentSoFar.Type == PaymentType.Module || lastPaymentSoFar.Type == PaymentType.Month)
                            && pureBalance >= toDecrease)
                            {
                                pureBalance -= toDecrease;
                                currentBalance -= toDecrease;
                                t = true;
                            }
                            else
                            {
                                toDecrease = module.LessonPrice;
                            }
                        }
                        if (!t)
                        {
                            pureBalance -= toDecrease;
                            currentBalance -= toDecrease;
                        }
                    }
                    else if (journal.Assessment.Type == Constants.NotRespectfulReason)
                    {
                     
                        if (lastPaymentSoFar != null)
                        {
                            // Важный момент - если у студента осталось меньше 
                            // чем на занятие, то это значит что все проплаченные месячные занятия 
                            // кончились и снимать деньги не надо
                            if ((lastPaymentSoFar.Type == PaymentType.Module || lastPaymentSoFar.Type == PaymentType.Month)
                            && pureBalance >= toDecrease)
                            {
                                pureBalance -= toDecrease;
                                currentBalance -= toDecrease;
                            }
                        }
                    }
                }
                
                
                double returnSum = _context.Students.First(x => x.ID == ID).Returns.Sum(x => x.Sum);
                Balance = pureBalance - returnSum;
                _context.Students.First(x => x.ID == ID).Balance = Balance;
                _context.SaveChanges();
            }
        }
 
        public Payment LastPayment(DateTime date) {
            date = date.Date;
            using (var _context = new ApplicationContext())
            {
                return _context.Students.First(x => x.ID == ID).Payments  // уточнить
                    .Where(x => x.Date.Date <= date)
                    .OrderBy(x => x.Date)
                    .LastOrDefault();
            }
}

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

        public virtual Module Module
        {
            get
            {
                return _Module;
            }
            set
            {
                _Module = value;
                OnPropertyChanged("Module");
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

        public double Balance
        {
            get
            {
                return _Balance;
            }
            set
            {
                _Balance = value;
                OnPropertyChanged("Balance");
            }
        }

        public override string ToString()
        {
            return Person.FullName;
        }


    }
}
