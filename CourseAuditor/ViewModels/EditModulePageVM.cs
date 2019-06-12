using CourseAuditor.DAL;
using CourseAuditor.Helpers;
using CourseAuditor.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CourseAuditor.ViewModels
{
    public class EditModulePageVM : BaseVM, IPageVM
    {
        public EditModulePageVM(Module selectedModule = null)
        {
            Persons = new ObservableCollection<CheckedListItem<Person>>();
            using (var _context = new ApplicationContext())
            {
                Courses = new ObservableCollection<Course>(_context.Courses.Include(x => x.Groups.Select(t => t.Modules)));
            }

            if (selectedModule != null)
            {
                SelectedModule = selectedModule;
                SelectedGroup = selectedModule.Group;
                SelectedCourse = SelectedGroup.Course;
            }
            else
            {
                SelectedCourse = Courses.FirstOrDefault();
            }

            EventsManager.ObjectChangedEvent += (s, e) =>
            {
                if (e.ObjectChanged is Module)
                {
                    int? id = SelectedModule?.ID;
                    using (var _context = new ApplicationContext())
                    {
                        Courses = new ObservableCollection<Course>(_context.Courses.Include(x => x.Groups.Select(t => t.Modules)));
                        if (id != null)
                        {
                            SelectedModule = _context.Modules.FirstOrDefault(x => x.ID == id);
                            SelectedGroup = SelectedModule?.Group;
                            SelectedCourse = SelectedGroup?.Course;
                            if (SelectedCourse == null)
                            {
                                SelectedCourse = Courses.FirstOrDefault();
                            }
                        }
                        else
                        {
                            SelectedCourse = Courses.FirstOrDefault();
                        }
                    }
                }
                else { 
}
            };
        }

        private void LoadPersons(Module selectedModule)
        {
            using (var _context = new ApplicationContext())
            {
                Persons.Clear();
                if (selectedModule != null)
                {
                    
                    List<Person> selectedPersons = new List<Person>();
                    List<Person> unselectedPersons = new List<Person>();
                    selectedPersons = _context.Modules
                        .First(x => x.ID == selectedModule.ID).Students
                        .Select(x => x.Person)
                        .OrderBy(x => x.SecondName)
                        .ToList();

                    var temp = _context.Persons.ToList();
                    unselectedPersons = temp.Where(x => !selectedPersons.Any(y => x.ID == y.ID))
                        .OrderBy(x => x.SecondName)
                        .ToList();

                    foreach (var person in selectedPersons)
                    {
                        Persons.Add(new CheckedListItem<Person>(person, true));
                    }

                    foreach (var person in unselectedPersons)
                    {
                        Persons.Add(new CheckedListItem<Person>(person, false));
                    }
                }
                else
                {
                    var persons = _context.Persons.OrderBy(x => x.SecondName).ToList();
                    foreach (var person in persons)
                    {
                        Persons.Add(new CheckedListItem<Person>(person));
                    }
                }
            }
        }

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
                if (_SelectedGroup != null)
                {
                    Modules = new ObservableCollection<Module>(_SelectedGroup.Modules);
                    if (SelectedModule == null)
                        SelectedModule = _SelectedGroup.LastModule;
                }
                else
                {
                    SelectedModule = null;
                }
                OnPropertyChanged("SelectedGroup");
            }
        }

        private ObservableCollection<Module> _Modules;
        public ObservableCollection<Module> Modules
        {
            get
            {
                return _Modules;
            }
            set
            {
                _Modules = value;
                OnPropertyChanged("Modules");
            }
        }

        private Module _SelectedModule;
        public Module SelectedModule
        {
            get
            {
                return _SelectedModule;
            }
            set
            {
                _SelectedModule = value;
                if (_SelectedModule != null)
                {
                    DateStart = _SelectedModule.DateStart;
                    DateEnd = _SelectedModule.DateEnd;
                    ModuleNumber = _SelectedModule.Number;
                    LessonCount = _SelectedModule.LessonCount;
                    LessonPrice = _SelectedModule.LessonPrice;
                    LoadPersons(_SelectedModule);
                }
                OnPropertyChanged("SelectedModule");
            }
        }

      

        private ObservableCollection<CheckedListItem<Person>> _Persons;
        public ObservableCollection<CheckedListItem<Person>> Persons
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

        private DateTime? _DateEnd;
        public DateTime? DateEnd
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

        private int _ModuleNumber;
        public int ModuleNumber
        {
            get
            {
                return _ModuleNumber;
            }
            set
            {
                _ModuleNumber = value;
                OnPropertyChanged("ModuleNumber");
            }
        }

        private int _LessonCount;
        public int LessonCount
        {
            get
            {
                return _LessonCount;
            }
            set
            {
                _LessonCount = value;
                OnPropertyChanged("LessonCount");
            }
        }

        private double _LessonPrice;
        public double LessonPrice
        {
            get
            {
                return _LessonPrice;
            }
            set
            {
                _LessonPrice = value;
                OnPropertyChanged("LessonPrice");
            }
        }

      
        private void UpdateModule()
        {
            if (SelectedModule != null)
            {
                using (var _context = new ApplicationContext())
                {
                    var module = _context.Modules.Include(x => x.Students).FirstOrDefault(x => x.ID == SelectedModule.ID);
                    if (module != null)
                    {
                        module.Number = ModuleNumber;
                        module.LessonPrice = LessonPrice;
                        module.LessonCount = LessonCount;
                        module.DateStart = DateStart;
                        module.DateEnd = DateEnd;


                        _context.Students.RemoveRange(module.Students);
                  
                        foreach (var person in Persons.Where(x => x.IsChecked))
                        {
                            Student student = new Student()
                            {
                                DateStart = DateStart,
                                Person_ID = person.Item.ID,
                                Module_ID = SelectedModule.ID
                            };
                            _context.Students.Add(student);
                            _context.Entry(student).State = EntityState.Added;
                        }
                        _context.SaveChanges();
                    }
                }
                EventsManager.RaiseObjectChangedEvent(SelectedModule);
            }
        }
            
        private ICommand _UpdateModuleCommand;
        public ICommand UpdateModuleCommand =>
            _UpdateModuleCommand ??
            (_UpdateModuleCommand = new RelayCommand(
                (obj) =>
                {
                    UpdateModule();
                }
                ));

        private ICommand _DeleteModuleCommand;
        public ICommand DeleteModuleCommand =>
            _DeleteModuleCommand ??
            (_DeleteModuleCommand = new RelayCommand(
                (obj) =>
                {

                }
                ));
    }
}
