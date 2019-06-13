using CourseAuditor.DAL;
using CourseAuditor.Helpers;
using CourseAuditor.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Windows.Input;

namespace CourseAuditor.ViewModels
{
    public class AddStudentPageVM : BaseVM, IPageVM
    {
        public AddStudentPageVM(Group selectedGroup = null)
        {
            Person = new Person();
            AddNewMode = true;
            using (var _context = new ApplicationContext())
            {
                Courses = new ObservableCollection<Course>(_context.Courses.Include(x => x.Groups.Select(t => t.Modules)));
                Persons = new ObservableCollection<Person>(_context.Persons.OrderBy(x => x.SecondName));
                SelectedPerson = Persons.FirstOrDefault();
            }

            if (selectedGroup != null)
            {
                SelectedGroup = selectedGroup;
                SelectedCourse = selectedGroup.Course;
            }
            else
            {
                SelectedCourse = Courses.FirstOrDefault();
            }
        }

        public bool AddNewMode { get; set; }

        private Module LastModule { get; set; }

        private ObservableCollection<Course> _Courses;
        public ObservableCollection<Course> Courses
        {
            get
            {
                return _Courses;
            }
            set
            {
                _Courses = value;
                OnPropertyChanged("Courses");
            }
        }

        private Course _SelectedCourse;
        public Course SelectedCourse
        {
            get
            {
                return _SelectedCourse;
            }
            set
            {
                _SelectedCourse = value;
                if (_SelectedCourse != null)
                {
                    Groups = new ObservableCollection<Group>(_SelectedCourse.Groups);
                    if (SelectedGroup == null)
                        SelectedGroup = Groups?.FirstOrDefault();
                }
                else
                {
                    Groups = new ObservableCollection<Group>();
                }
                OnPropertyChanged("SelectedCourse");
            }
        }

        private ObservableCollection<Group> _Groups;
        public ObservableCollection<Group> Groups
        {
            get
            {
                return _Groups;
            }
            set
            {
                _Groups = value;
                OnPropertyChanged("Groups");
            }
        }

        private Group _SelectedGroup;
        public Group SelectedGroup
        {
            get
            {
                return _SelectedGroup;
            }
            set
            {
                _SelectedGroup = value;
                OnPropertyChanged("SelectedGroup");
                if (_SelectedGroup != null)
                {
                    if (_SelectedGroup.Modules.Count > 0)
                    {
                        LastModule = _SelectedGroup.LastModule;
                    }
                    else LastModule = null;
                }

            }
        }

        private ObservableCollection<Person> _Persons;
        public ObservableCollection<Person> Persons
        {
            get
            {
                return _Persons;
            }
            set
            {
                _Persons = value;
                OnPropertyChanged("Persons");
            }
        }

        private Person _SelectedPerson;
        public Person SelectedPerson
        {
            get
            {
                return _SelectedPerson;
            }
            set
            {
                _SelectedPerson = value;
                OnPropertyChanged("SelectedPerson");
            }
        }

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

        public ObservableCollection<Parent> Parents { get; set; }

        private void AddStudent()
        {
            if (!AddNewMode) Person = SelectedPerson;

            if (Person != null && LastModule != null)
            {
                using (var _context = new ApplicationContext())
                {
                    var person = _context.Persons.FirstOrDefault(x => x.ID == Person.ID);
                    if (person == null)
                    {
                        person = new Person()
                        {
                            FirstName = Person.FirstName,
                            SecondName = Person.SecondName,
                            Patronymic = Person.Patronymic,
                            Phone = Person.Phone,
                            // TODO Parents
                        };
                        _context.Persons.Add(person);
                        _context.SaveChanges();
                    }
                    var student = new Student()
                    {
                        DateStart = LastModule.DateStart,
                        Balance = 0,
                        Module_ID = LastModule.ID,
                        Person_ID = person.ID
                    };
                    _context.Students.Add(student);
                    _context.SaveChanges();
                    EventsManager.RaiseObjectChangedEvent(student, ChangeType.Added);
                }

            }
        }

        private ICommand _AddStudentCommand;
        public ICommand AddStudentCommand =>
            _AddStudentCommand ??
            (_AddStudentCommand = new RelayCommand(
                (obj) =>
                {
                    AddStudent();
                }
                ));
    }
}
