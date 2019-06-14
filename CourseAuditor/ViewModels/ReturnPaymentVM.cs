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
    public class ReturnPaymentVM : BaseVM, IViewVM
    {

        public ReturnPaymentVM(IView view, Student student)
        {
            SelectedStudent = student;
            CurrentView = view;
            CurrentView.DataContext = this;
            Balance = SelectedStudent.Balance;
            Sum = 0;
            InfBalance = "Баланс: " + Balance;
            InfStudent = GetInfStud();
            CurrentView.Show();
        }


        private IPageVM _CurrentPageVM;
        public IPageVM CurrentPageVM
        {
            get
            {
                return _CurrentPageVM;
            }
            set
            {
                _CurrentPageVM = value;
                OnPropertyChanged("CurrentPageVM");
            }
        }
        public IView CurrentView  { get; set; }

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

        private double _Sum;
        public double Sum
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

        private string _InfStudent;
        public string InfStudent
        {
            get
            {
                return _InfStudent;
            }
            set
            {
                _InfStudent = value;
                OnPropertyChanged("InfStudent");
            }
        }

        private double _Balance;
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

        private string _InfBalance;
        public string InfBalance
        {
            get
            {
                return _InfBalance;
            }
            set
            {
                _InfBalance = value;
                OnPropertyChanged("InfBalance");
            }
        }
        private string GetInfStud()
        {
          return  $"{SelectedStudent.Person.FullName} " +
                 $"Курс {SelectedStudent.Module.Group.Course.Name} " +
                 $"Группа {SelectedStudent.Module.Group.Title} " +
                 $"Модуль {SelectedStudent.Module.Number}";
        }

        private RelayCommand _AddReturnCommand;
        public RelayCommand AddReturnCommand =>
            _AddReturnCommand ??
            (_AddReturnCommand = new RelayCommand(
                (obj) =>
                {
                    AddReturn();
                },
                (obj) =>
                {
                    return true;
                }
        ));

        private void AddReturn()
        {
            
            using (var _context = new ApplicationContext())
            {
                SelectedStudent.Balance -=Sum;
                var @return = new Return();
                @return.Student_ID = SelectedStudent.ID;
                @return.Sum = Sum;
                @return.Date = DateTime.Now;
                _context.Returns.Add(@return);
                var student = _context.Students.First(x => x.ID == SelectedStudent.ID);
                _context.Entry(student).CurrentValues.SetValues(SelectedStudent);
                _context.SaveChanges();
            }
        }


    }
}
