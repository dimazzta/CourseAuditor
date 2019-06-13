using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourseAuditor.DAL;
using CourseAuditor.Models;
using CourseAuditor.Views;
using System.Data.Entity;
using CourseAuditor.Helpers;

namespace CourseAuditor.ViewModels
{
    public class AddMedicalDocVM : BaseVM, IViewVM
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

        private string _InfoStudent;
        public string InfoStudent
        {
            get
            {
                return $"{_InfoStudent}";
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

        private void AddMedicalDoc()
        {
            Assessment RespectfulReason = new Assessment();
            
            var MedicalDoc = new MedicalDoc();
            MedicalDoc.Comment = Comment;
            MedicalDoc.DateStart = DateStart;
            MedicalDoc.DateEnd = DateEnd;
            MedicalDoc.Person_ID = SelectedStudent.Person.ID;

            using (var _context = new ApplicationContext())
            {
                Journals = new ObservableCollection<Journal>(_context.Journals
                    .Where(x => x.Date >= DateStart && x.Date <= DateEnd)
                    .Where(x => x.Student.Person_ID == SelectedStudent.Person.ID)
                    .Include(x => x.Student)
                    .Include(x=>x.Assessment));

                var UpdateJournals = Journals;

                RespectfulReason = _context.Assessments.Where(x => x.Type == 2).FirstOrDefault();

                foreach (var item in UpdateJournals)
                {
                    item.Assessment = RespectfulReason;
                }
                for (int i = 0; i < Journals.Count; i++)
                {
                    _context.Entry(Journals[i]).CurrentValues.SetValues(UpdateJournals[i]);
                }
                _context.SaveChanges();
            }

        }

        private RelayCommand _AddMedicalDocCommand;
        public RelayCommand AddMedicalDocCommand =>
            _AddMedicalDocCommand ??
            (_AddMedicalDocCommand = new RelayCommand(
                (obj) =>
                {
                    AddMedicalDoc();
                },
                (obj) =>
                {
                    return true;
                }
        ));

        public AddMedicalDocVM(IView view, Student selectedStudent)
        {
            DateStart = DateTime.Now;
            DateEnd = DateTime.Now;
            SelectedStudent = selectedStudent;
            _InfoStudent = $"{SelectedStudent.Person.FullName}";
            CurrentView = view;
            CurrentView.DataContext = this;
            CurrentView.Show();
        }
    }
}
