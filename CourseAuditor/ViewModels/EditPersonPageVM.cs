using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using CourseAuditor.DAL;
using CourseAuditor.Helpers;
using CourseAuditor.Models;
using System.Data.Entity;
using System.Windows;

namespace CourseAuditor.ViewModels
{
    class EditPersonPageVM : BaseVM, IPageVM, IEditPageVM
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

        private ParentPickerVM _ParentPicker;
        public ParentPickerVM ParentPicker
        {
            get
            {
                return _ParentPicker;
            }
            set
            {
                _ParentPicker = value;
                OnPropertyChanged("ParentPicker");
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
                if (_SelectedStudent != null)
                {
                    Payments = new ObservableCollection<Payment>(_SelectedStudent.Payments.OrderByDescending(x => x.Date));
                    Returns = new ObservableCollection<Return>(_SelectedStudent.Returns.OrderByDescending(x => x.Date));
                    MedicalDocs = new ObservableCollection<MedicalDoc>(_SelectedStudent.Person.MedicalDocs.OrderByDescending(x => x.DateStart));
                }
                else
                {
                    Payments = new ObservableCollection<Payment>();
                    Returns = new ObservableCollection<Return>();
                    MedicalDocs = new ObservableCollection<MedicalDoc>();
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

        public static void DeleteStudent(Student student)
        {
            if (student != null)
            {
                using (var _context = new ApplicationContext())
                {
                    var deleted = _context.Students.Include(x => x.Module.Group.Course).First(x => x.ID == student.ID);
                    var module = student.Module;
                    var group = module.Group;
                    if (deleted != null)
                    {
                        var f = MessageBox.Show($"Вы действительно хотите удалить {deleted.Person.FullName} из {group.Title}, {module}?", "Подтверждение удаления", MessageBoxButton.YesNo);
                        if (f == MessageBoxResult.No)
                        {
                            return;
                        }
                        _context.Students.Remove(deleted);
                        _context.SaveChanges();
                        AppState.I.SelectedContextStudent = null;
                        EventsManager.RaiseObjectChangedEvent(deleted, ChangeType.Deleted);
                    }
                }
            }
        }

     
        public static void DeletePerson(Person person)
        {
            if (person != null)
            {
                using (var _context = new ApplicationContext())
                {
                    var deleted = _context.Persons
                        .Include(x => x.Students
                        .Select(t => t.Module.Group.Course))
                        .First(x => x.ID == person.ID);

                    var groupCount = deleted.Students.Count;
                    if (deleted != null)
                    {
                        var f = MessageBox.Show($"Вы действительно хотите удалить {deleted.FullName}? Данный студент состоит в {groupCount} группах", "Подтверждение удаления", MessageBoxButton.YesNo);
                        if (f == MessageBoxResult.No)
                        {
                            return;
                        }
                        _context.Persons.Remove(deleted);
                        _context.SaveChanges();
                        AppState.I.SelectedContextStudent = null;
                        EventsManager.RaiseObjectChangedEvent(deleted, ChangeType.Deleted);
                    }
                }
            }
        }

        void LoadData(Person person)
        {
            Person = person;
            if (person != null)
            {
                using (ApplicationContext _context = new ApplicationContext())
                {
                    Students = new ObservableCollection<Student>(
                        _context.Students.Where(x => x.Person.ID == person.ID)
                        .Include(x => x.Payments)
                        .Include(x => x.Returns)
                        .Include(x => x.Person.MedicalDocs)
                        .Include(x => x.Module.Group.Course));

                    SelectedStudent = Students.FirstOrDefault();

                    var parents = _context.PersonParents.Where(x => x.Person_ID == person.ID).Select(x => x.Parent).ToList();
                    ParentPicker = new ParentPickerVM(parents);
                }
            }
        }

        public EditPersonPageVM(ICommand goBack, Student student)
        {
            GoBack = goBack;
            LoadData(student.Person);
            EventsManager.ObjectChangedEvent += EventsManager_ObjectChangedEvent;
        }

        private void EventsManager_ObjectChangedEvent(object sender, ObjectChangedEventArgs e)
        {
            if (e.Type == ChangeType.Deleted)
            {
                if (e.ObjectChanged is Group || e.ObjectChanged is Course || e.ObjectChanged is Module || e.ObjectChanged is Student)
                {
                    LoadData(Person);
                }
                if (e.ObjectChanged is Person)
                {
                    if (Person.ID == (e.ObjectChanged as Person).ID)
                    {
                        object o = new object();
                        GoBack.Execute(o);
                    }
                }
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

        private ICommand _DeleteStudentCommand;
        public ICommand DeleteStudentCommand =>
            _DeleteStudentCommand ??
            (_DeleteStudentCommand = new RelayCommand(
                (obj) =>
                {
                    DeleteStudent(SelectedStudent);
                }
                ));

        private ICommand _DeletePersonCommand;
        public ICommand DeletePersonCommand =>
            _DeletePersonCommand ??
            (_DeletePersonCommand = new RelayCommand(
                (obj) =>
                {
                    DeletePerson(Person);
                }
                ));

        public ICommand GoBack { get; set; }
    }
}
