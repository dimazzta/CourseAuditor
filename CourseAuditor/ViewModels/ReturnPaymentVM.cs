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

namespace CourseAuditor.ViewModels
{
    public class ReturnPaymentVM : BaseVM, IDialogRequestClose
    {
        public ReturnPaymentVM(Student student)
        {
            SelectedStudent = student;
            using(var _context = new ApplicationContext())
            {
                SelectedStudent = _context.Students
                    .Include(x => x.Person)
                    .Include(x => x.Module.Group.Course)
                    .FirstOrDefault(x => x.ID == student.ID);
            }
            Sum = 0;
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

        private double _Sum;
        public double Sum
        {
            get
            {
                return _Sum;
            }
            set
            {
                try
                {
                    _Sum = Convert.ToDouble(value);
                }
                catch
                {
                    _Sum = 0;
                }
                OnPropertyChanged("Sum");
            }
        }

        private ICommand _AddReturnCommand;
        public ICommand AddReturnCommand =>
            _AddReturnCommand ??
            (_AddReturnCommand = new RelayCommand(
                (obj) =>
                {
                    AddReturn();
                    CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(true));
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
                SelectedStudent.Balance -= Sum;
                var @return = new Return();
                @return.Student_ID = SelectedStudent.ID;
                @return.Sum = Sum;
                @return.Date = DateTime.Now;
                _context.Returns.Add(@return);
                var student = _context.Students.First(x => x.ID == SelectedStudent.ID);
                _context.Entry(student).CurrentValues.SetValues(SelectedStudent);
                _context.SaveChanges();

                EventsManager.RaiseObjectChangedEvent(@return, ChangeType.Added);
            }
        }

    }
}
