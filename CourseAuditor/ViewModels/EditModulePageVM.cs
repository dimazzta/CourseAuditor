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
using System.Windows;

namespace CourseAuditor.ViewModels
{
    public class EditModulePageVM : BaseVM, IPageVM, IEditPageVM
    {
        public EditModulePageVM(ICommand goBack, Module selectedModule = null)
        {
            GoBack = goBack;
            Persons = new ObservableCollection<CheckedListItem<Person>>();
            if (selectedModule != null)
            {
                SelectedModule = selectedModule;
                ModuleNumber = selectedModule.Number.ToString();
                LessonPrice = selectedModule.LessonPrice.ToString();
                LessonCount = selectedModule.LessonCount.ToString();
                DateStart = selectedModule.DateStart;
                DateEnd = selectedModule.DateEnd.Value;

                SelectedGroup = SelectedModule.Group;
                SelectedCourse = SelectedGroup.Course;
            }

            EventsManager.ObjectChangedEvent += (s, e) =>
            {
                if ((e.ObjectChanged is Group || e.ObjectChanged is Course || e.ObjectChanged is Module) && e.Type == ChangeType.Deleted)
                {
                    if (e.ObjectChanged is Course && (e.ObjectChanged as Course).ID == SelectedCourse?.ID
                        || e.ObjectChanged is Group &&  (e.ObjectChanged as Group).ID == SelectedGroup?.ID
                        || e.ObjectChanged is Module &&  (e.ObjectChanged as Module).ID == SelectedModule?.ID
                        || SelectedModule == null)
                    {
                        object o = new object();
                        GoBack.Execute(o);
                    }
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
                        .FirstOrDefault(x => x.ID == selectedModule.ID).Students?
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
                OnPropertyChanged("SelectedCourse");
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
                LoadPersons(_SelectedModule);
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

        private string _ModuleNumber;
        public string ModuleNumber
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

        private string _LessonCount;
        public string LessonCount
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

        private string _LessonPrice;
        public string LessonPrice
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

            if (!Int32.TryParse(ModuleNumber, out int n))
            {
                err.Append("*Номер модуля должен быть числом. \n");
            }

            int lc;
            if (!Int32.TryParse(LessonCount, out lc))
            {
                err.Append("*Количество занятий должно быть числом. \n");
            }
            else if (lc <= 0)
            {
                err.Append("*Количество занятий должно быть больше нуля. \n");
            }

            double d;
            if (!Double.TryParse(LessonPrice, out d))
            {
                err.Append("*Цена занятия должна быть целым или вещественным числом. \n");
            }
            else if (d <= 0)
            {
                err.Append("*Цена занятия должна быть больше нуля. \n");
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
        private void UpdateModule()
        {
            if (SelectedModule != null)
            {
                using (var _context = new ApplicationContext())
                {
                    var module = _context.Modules.Include(x => x.Students.Select(t => t.Person)).FirstOrDefault(x => x.ID == SelectedModule.ID);
                    if (module != null)
                    {
                        module.Number = int.Parse(ModuleNumber);
                        module.LessonPrice = double.Parse(LessonPrice);
                        module.LessonCount = int.Parse(LessonCount);
                        module.DateStart = DateStart;
                        module.DateEnd = DateEnd;

                        List<Student> toDelete = new List<Student>();
                        foreach(var person in Persons.Where(x => !x.IsChecked))
                        {
                            toDelete.AddRange(module.Students.Where(x => x.Person.ID == person.Item.ID).ToList());
                        }
                        _context.Students.RemoveRange(toDelete);
                        _context.SaveChanges();
                        foreach (var person in Persons.Where(x => x.IsChecked))
                        {
                            if (!module.Students.Select(x => x.Person).Any(x => x?.ID == person.Item.ID))
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
                        }
                        _context.SaveChanges();
                    }
                }
                EventsManager.RaiseObjectChangedEvent(SelectedModule, ChangeType.Updated);
            }
        }

        public static void DeleteModule(Module selectedModule)
        {
            if (selectedModule != null)
            {
                if (selectedModule.Students.Count != 0)
                {
                    var f = MessageBox.Show("Модуль не пуст. Вы уверены, что хотите удалить его?", "Модуль не пуст", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (f == MessageBoxResult.No)
                    {
                        return;
                    }
                }
                using (var _context = new ApplicationContext())
                {
                    var deleted = _context.Modules.First(x => x.ID == selectedModule.ID);
                    if (deleted != null)
                    {
                        _context.Modules.Remove(deleted);
                        _context.SaveChanges();
                    }
                }
                AppState.I.SelectedContextModule = null;
                EventsManager.RaiseObjectChangedEvent(selectedModule, ChangeType.Deleted);
            }
        }

        private ICommand _UpdateModuleCommand;
        public ICommand UpdateModuleCommand =>
            _UpdateModuleCommand ??
            (_UpdateModuleCommand = new RelayCommand(
                (obj) =>
                {
                    if (Validate())
                        UpdateModule();
                }
                ));

        private ICommand _DeleteModuleCommand;
        public ICommand DeleteModuleCommand =>
            _DeleteModuleCommand ??
            (_DeleteModuleCommand = new RelayCommand(
                (obj) =>
                {
                    DeleteModule(SelectedModule);
                }
                ));

        public ICommand GoBack { get; set; }

        public string PageTitle => "Редактирование модуля";
    }
}
