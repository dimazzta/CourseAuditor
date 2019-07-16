using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourseAuditor.DAL;
using CourseAuditor.Helpers;
using CourseAuditor.Models;
using CourseAuditor.ViewModels.Dialogs;
using CourseAuditor.Views;
using System.Data.Entity;
using System.Windows.Input;
using System.Windows;
using System.Globalization;

namespace CourseAuditor.ViewModels
{
    public class ReturnPaymentVM : BaseVM, IDialogRequestClose
    {
        public ReturnPaymentVM(Student student)
        {
            using(var _context = new ApplicationContext())
            {
                SelectedStudent = _context.Students
                    .Include(x => x.Person)
                    .Include(x => x.Module.Group.Course)
                    .FirstOrDefault(x => x.ID == student.ID);
            }
            Sum = "0";
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
                LoadStudentInfo();
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
                OnPropertyChanged("StudentInfo");
            }
        }

        void LoadStudentInfo()
        {
            StudentInfo = $"Студент: {SelectedStudent.Person.FullName}.\n" +
                $"Курс: {SelectedStudent.Module.Group.Course.Name}.\n" +
                $"Группа: {SelectedStudent.Module.Group.Title}.\n" +
                $"Модуль: {SelectedStudent.Module.Number}.\n" +
                $"Текущий баланс: {SelectedStudent.Balance}";
        }

        private string _Sum;
        public string Sum
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


        private string _Error;
        public string Error
        {
            get
            {
                return _Error;
            }
            set
            {
                _Error = value;
                OnPropertyChanged("Error");
            }
        }

        public bool Validate()
        {
            StringBuilder err = new StringBuilder();
            double d;
            if (!double.TryParse(Sum, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out d))
            {
                err.Append("*Сумма не может быть пустой. \n");
            }
            else if (d <= 0)
            {
                err.Append("*Сумма возврата должна быть больше нуля. \n");
            }

            Error = err.ToString();
            if (err.Length == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private ICommand _AddReturnCommand;
        public ICommand AddReturnCommand =>
            _AddReturnCommand ??
            (_AddReturnCommand = new RelayCommand(
                (obj) =>
                {
                    if (Validate())
                    {
                        AddReturn();
                        CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(true));
                    }
                    
                },
                (obj) =>
                {
                    return true;
                }
        ));

        public event EventHandler<DialogCloseRequestedEventArgs> CloseRequested;

        private void AddReturn()
        {
            using (var _context = new ApplicationContext())
            {
                if (SelectedStudent.Balance >= double.Parse(Sum, CultureInfo.InvariantCulture))
                {
                    SelectedStudent.Balance -= double.Parse(Sum, CultureInfo.InvariantCulture);
                    var @return = new Return();
                    @return.Student_ID = SelectedStudent.ID;
                    @return.Sum = double.Parse(Sum, CultureInfo.InvariantCulture);
                    @return.Date = DateTime.Now;
                    _context.Returns.Add(@return);
                    var student = _context.Students.First(x => x.ID == SelectedStudent.ID);
                    _context.Entry(student).CurrentValues.SetValues(SelectedStudent);
                    _context.SaveChanges();

                    EventsManager.RaiseObjectChangedEvent(@return, ChangeType.Added);
                }
                else
                {
                    MessageBox.Show("Невозможно оформить вовзрат. Баланс студента с учетом возврата отрицательный.");
                }
            }
        }

    }
}
