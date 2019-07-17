using System;
using System.Collections.ObjectModel;
using System.Linq;
using CourseAuditor.DAL;
using CourseAuditor.Models;
using System.Data.Entity;
using CourseAuditor.Helpers;
using System.Windows.Input;
using CourseAuditor.ViewModels.Dialogs;
using System.Text;

namespace CourseAuditor.ViewModels
{
    public class AddMedicalDocVM : BaseVM, IDialogRequestClose
    {

        private ObservableCollection<Journal> _Journals;
        public ObservableCollection<Journal> Journals
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

        private string _Comment;
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

        private DateTime _DateStart;
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

        private DateTime _DateEnd;
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

            if (DateTime.Compare(DateStart, DateEnd) > 0)
            {
                err.Append("*Дата начала не может быть позже даты окончания. \n");
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


        private void AddMedicalDoc()
        {
            var MedicalDoc = new MedicalDoc();
            MedicalDoc.Comment = Comment;
            MedicalDoc.DateStart = DateStart;
            MedicalDoc.DateEnd = DateEnd;
            MedicalDoc.Person_ID = SelectedStudent.Person.ID;

            using (var _context = new ApplicationContext())
            {
                var newEndDate = DateEnd.AddDays(1).Date;
                Journals = new ObservableCollection<Journal>(_context.Journals
                    .Where(x => x.Date >= DateStart.Date && x.Date < newEndDate)
                    .Where(x => x.Student.Person_ID == SelectedStudent.Person.ID)
                    .Where(x => x.Assessment.Type==1)
                    .Include(x => x.Student)
                    .Include(x=>x.Assessment));

                var RespectfulReason = _context.Assessments.Where(x => x.Type == 2).FirstOrDefault();

                foreach (var item in Journals)
                {
                    item.Assessment = RespectfulReason;
                    _context.Entry(item).State = EntityState.Modified;
                }
                _context.MedicalDocs.Add(MedicalDoc);
                _context.SaveChanges();
                EventsManager.RaiseObjectChangedEvent(MedicalDoc, ChangeType.Added);
            }
        }

        private ICommand _AddMedicalDocCommand;

        

        public ICommand AddMedicalDocCommand =>
            _AddMedicalDocCommand ??
            (_AddMedicalDocCommand = new RelayCommand(
                (obj) =>
                {
                    if (Validate())
                    {
                        AddMedicalDoc();
                        CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(true));
                    }
                },
                (obj) =>
                {
                    return true;
                }
        ));

        void LoadStudentInfo()
        {
            StudentInfo = $"Студент: {SelectedStudent.Person.FullName}.\n" +
               $"Курс: {SelectedStudent.Module.Group.Course.Name}.\n" +
               $"Группа: {SelectedStudent.Module.Group.Title}.\n" +
               $"Модуль: {SelectedStudent.Module.Number}.\n";
        }

        public AddMedicalDocVM(Student selectedStudent)
        {
            DateStart = DateTime.Now;
            DateEnd = DateTime.Now;
            using(var _context = new ApplicationContext())
            {
                SelectedStudent = _context.Students
                    .Include(x => x.Person)
                    .Include(x => x.Module.Group.Course)
                    .FirstOrDefault(x => x.ID == selectedStudent.ID);
            }
        }

        public event EventHandler<DialogCloseRequestedEventArgs> CloseRequested;
    }
}
