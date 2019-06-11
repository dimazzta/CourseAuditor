using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourseAuditor.DAL;
using CourseAuditor.Helpers;
using CourseAuditor.Models;
using CourseAuditor.Views;

namespace CourseAuditor.ViewModels
{
    public class PaymentVM: BaseVM, IViewVM
    {
        private IPageVM _CurrentPageVM;
        public IPageVM CurrentPageVM
        {
            get => _CurrentPageVM;
            set
            {
                _CurrentPageVM = value;
                OnPropertyChanged("CurrentPageVM");
            }
        }
        
        public IView CurrentView { get; set; }

        private ObservableCollection<Student> _Students;
        public ObservableCollection<Student> Students
        {
            get
            {
                return _Students;
            }
            set
            {
                _Students = value;
                OnPropertyChanged("Students");
            }
        }

        private ObservableCollection<Payment> _Payments;
        public ObservableCollection<Payment> Payments
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

        private Student _SelectedStudent;
        public Student SelectedStudent
        {
            get
            {
                return _SelectedStudent;
            }
            set
            {
                _SelectedStudent = value;
                OnPropertyChanged("SelectedStudent");
            }
        }

        private double? _Sum;
        public double? Sum
        {
            get
            {
                return _Sum;
            }
            set
            {
                _Sum = Convert.ToDouble(value);
                OnPropertyChanged("Sum");
            }
        }

        private double? _Discount;
        public double? Discount
        {
            get
            {
                return _Discount;
            }
            set
            {
                _Discount = Convert.ToDouble(value);
                OnPropertyChanged("Discount");
            }
        }

        private string _StudentInfo;
        public string StudentInfo
        {
            get
            {
                return _StudentInfo;
            }
            set
            {
                _StudentInfo = value;
                OnPropertyChanged(StudentInfo);
            }
        }

        private double _СurrentBalance;
        public double СurrentBalance
        {
            get
            {
                return _СurrentBalance;
            }
            set
            {
                _СurrentBalance = value;
                OnPropertyChanged("СurrentBalance");
            }
        }

        public string BalanceInfo
        {
            get
            {
                return $"Текущий баланс: {СurrentBalance}";
            }
        }

        private void AddPayment()
        {
            SelectedStudent.Balance = Sum.Value * Discount.Value;
            using (var _context = new ApplicationContext())
            {
                var payment = new Payment();
                payment.Discount = Discount.Value;
                payment.Sum = Sum.Value;
                payment.Student_ID = SelectedStudent.ID;
                payment.Date = DateTime.Now;
                _context.Payments.Add(payment);
                var student = _context.Students.First(x => x.ID == SelectedStudent.ID);
                _context.Entry(student).CurrentValues.SetValues(SelectedStudent);
                _context.SaveChanges();
            }
        }

        private RelayCommand _AddPaymentCommand;
        public RelayCommand AddPaymentCommand =>
            _AddPaymentCommand ??
            (_AddPaymentCommand = new RelayCommand(
                (obj) =>
                {
                    AddPayment();
                },
                (obj) =>
                {
                    return true;
                }
        ));

        public PaymentVM(IView view, Student selectedStudent)
        {
            Sum = 0;
            Discount = 0;
            SelectedStudent = selectedStudent;
            CurrentView = view;
            StudentInfo = $"{selectedStudent.Person.FullName} " +
                $"Курс {selectedStudent.Module.Group.Course.Name} " +
                $"Группа {selectedStudent.Module.Group.Title} " +
                $"Модуль {selectedStudent.Module.Number}";
            СurrentBalance = selectedStudent.Balance;
            using (var _context = new ApplicationContext())
            {
                Students = new ObservableCollection<Student>(_context.Students);
                Payments = new ObservableCollection<Payment>(_context.Payments);
            }
            CurrentView = view;
            CurrentView.DataContext = this;
            CurrentView.Show();
        }

    }
}
