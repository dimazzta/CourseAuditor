using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using CourseAuditor.DAL;
using CourseAuditor.Helpers;
using CourseAuditor.Models;
using System.Data.Entity;

namespace CourseAuditor.ViewModels
{
    class EditPersonPageVM : BaseVM, IPageVM
    {
        private Person _Person;
        public Person Person
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

        public ParentPickerVM ParentPicker { get; set; }

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
                if (_SelectedStudent != null)
                {
                    Payments = new ObservableCollection<Payment>(_SelectedStudent.Payments.OrderByDescending(x => x.Date));
                    Returns = new ObservableCollection<Return>(_SelectedStudent.Returns.OrderByDescending(x => x.Date));
                    MedicalDocs = new ObservableCollection<MedicalDoc>(_SelectedStudent.Person.MedicalDocs.OrderByDescending(x => x.DateStart));
                }
                OnPropertyChanged("SelectedStudent");
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

        private ObservableCollection<Return> _Returns;
        public ObservableCollection<Return> Returns
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

        private ObservableCollection<MedicalDoc> _MedicalDocs;
        public ObservableCollection<MedicalDoc> MedicalDocs
        {
            get
            {
                return _MedicalDocs;
            }
            set
            {
                _MedicalDocs = value;
                OnPropertyChanged("MedicalDocs");
            }
        }

        public EditPersonPageVM(Student student)
        {
            Person = new Person();
            using(ApplicationContext _context = new ApplicationContext())
            {
                Person = _context.Persons
                    .Include(t => t.Parents
                    .Select(m => m.Parent))
                    .FirstOrDefault(x => x.ID == student.Person_ID);

                Students = new ObservableCollection<Student>(
                    _context.Students.Where(x => x.Person.ID == Person.ID)
                    .Include(x => x.Payments)
                    .Include(x => x.Returns)
                    .Include(x => x.Person.MedicalDocs)
                    .Include(x => x.Module.Group.Course));

                SelectedStudent = Students.FirstOrDefault();

                var parents = _context.PersonParents.Where(x => x.Person_ID == Person.ID).Select(x => x.Parent).ToList();
                ParentPicker = new ParentPickerVM(parents);
            }
        }

        private void EditPerson()
        {
            using (var _context = new ApplicationContext())
            {
                var person = _context.Persons
                    .Include(t => t.Parents
                    .Select(m => m.Parent))
                    .FirstOrDefault(x => x.ID == Person.ID);

                _context.Entry(person).CurrentValues.SetValues(Person);
                _context.Entry(person).State = EntityState.Modified;

                foreach(var p in person.Parents.ToList())
                {
                    _context.Entry(p).State = EntityState.Deleted;
                }

                var parents = ParentPicker.Parents;
                foreach(var parent in parents)
                {
                    var p = _context.Parents.FirstOrDefault(x => x.ID == parent.ID);
                    if (p == null)
                    {
                        p = _context.Parents.Add(parent);
                        _context.Entry(p).State = EntityState.Added;
                    }
                    else
                    {
                        _context.Entry(p).State = EntityState.Unchanged;
                    }
                   
                    var pp = _context.PersonParents.Add(new PersonParent()
                    {
                        Parent = p,
                        Person = person
                    });
                    _context.Entry(pp).State = EntityState.Added;
                }
                _context.SaveChanges();
            }
            EventsManager.RaiseObjectChangedEvent(Person, ChangeType.Updated);
        }


        private ICommand _EditPersonCommand;
        public ICommand EditPersonCommand =>
            _EditPersonCommand ??
            (_EditPersonCommand = new RelayCommand(
                (obj) =>
                {
                    EditPerson();
                }
                ));
    }
}
