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
    public class AddModulePageVM : BaseVM, IPageVM, IValidatable
    {
        void LoadData(Group selectedGroup = null)
        {
            Persons = new ObservableCollection<CheckedListItem<Person>>();
            using (var _context = new ApplicationContext())
            {
                Courses = new ObservableCollection<Course>(_context.Courses.Include(x => x.Groups.Select(t => t.Modules)));
                var persons = _context.Persons.OrderBy(x => x.SecondName).ToList();
                foreach (var person in persons)
                {
                    Persons.Add(new CheckedListItem<Person>(person));
                }
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

            CalculateDateBounds();
        }

        public AddModulePageVM(Group selectedGroup = null)
        {
            LoadData(selectedGroup);
            EventsManager.ObjectChangedEvent += EventsManager_ObjectChangedEvent;
        }

        private void EventsManager_ObjectChangedEvent(object sender, ObjectChangedEventArgs e)
        {
            if (e.ObjectChanged is Course || e.ObjectChanged is Group || e.ObjectChanged is Module || e.ObjectChanged is Person)
            {
                LoadData(null);
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
                    _LessonCount = _SelectedCourse.LessonCount.ToString();
                    _LessonPrice = _SelectedCourse.LessonPrice.ToString();
                }
                else
                {
                    Groups = new ObservableCollection<Group>();
                }
                OnPropertyChanged("SelectedCourse");
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
                        ModuleNumber = (_SelectedGroup.Modules.OrderBy(x => x.Number).Select(x => x.Number).Last() + 1).ToString();
                    }
                    else ModuleNumber = "1";
                }

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

        private void CalculateDateBounds()
        {
            DateStart = DateTime.Now;
            DateEnd = DateStart.AddMonths(Constants.DefaultModuleLengthMonth);
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

        private void AddModule()
        {
            if (SelectedGroup != null)
            {
                using (var _context = new ApplicationContext())
                {
                    var module = new Module()
                    {
                        DateStart = DateStart,
                        DateEnd = DateEnd,
                        Group_ID = SelectedGroup.ID,
                        Number = int.Parse(ModuleNumber),
                        LessonCount = int.Parse(LessonCount),
                        LessonPrice = int.Parse(LessonPrice)
                    };
                    var added = _context.Modules.Add(module);
                    _context.SaveChanges();
                    foreach (var person in Persons.Where(x => x.IsChecked))
                    {
                        Student student = new Student()
                        {
                            DateStart = DateStart,
                            Person_ID = person.Item.ID,
                            Module_ID = added.ID
                        };
                        _context.Students.Add(student);
                        _context.Entry(student).State = EntityState.Added;
                    }
                    _context.SaveChanges();
                    EventsManager.RaiseObjectChangedEvent(module, ChangeType.Added);
                }
            }
            
        }

        private ICommand _AddModuleCommand;
        public ICommand AddModuleCommand =>
            _AddModuleCommand ??
            (_AddModuleCommand = new RelayCommand(
                (obj) =>
                {
                    bool canAdd = SelectedGroup?.LastModule == null;
                    if (canAdd)
                    {
                        if (Validate())
                            AddModule();
                    }
                    else
                    {
                        MessageBox.Show("Невозможно добавить новый модуль, поскольку данная группа уже содержит открытый модуль. Закройте его и попробуйте снова.", "Невозможно добавить модуль", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                }
                ));

        public string PageTitle => "Добавление модуля";
    }
}
