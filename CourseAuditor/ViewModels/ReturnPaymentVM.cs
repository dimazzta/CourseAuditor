using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private RelayCommand _AddReturntCommand;
        public RelayCommand AddReturntCommand =>
            _AddReturntCommand ??
            (_AddReturntCommand = new RelayCommand(
                (obj) =>
                {
                    //AddPayment();
                },
                (obj) =>
                {
                    return true;
                }
        ));


    }
}
