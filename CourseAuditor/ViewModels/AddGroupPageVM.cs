using CourseAuditor.DAL;
using CourseAuditor.Helpers;
using CourseAuditor.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CourseAuditor.ViewModels
{
    public class AddGroupPageVM : BaseVM, IPageVM
    {
        void LoadData(Course selectedCourse = null)
        {
            using (var _context = new ApplicationContext())
            {
                Courses = new ObservableCollection<Course>(_context.Courses);
            }
            if (selectedCourse != null)
            {
                SelectedCourse = selectedCourse;
            }
            else
            {
                SelectedCourse = Courses.FirstOrDefault();
            }
        }

        public AddGroupPageVM(Course selectedCourse=null)
        {
            LoadData(selectedCourse);
            EventsManager.ObjectChangedEvent += EventsManager_ObjectChangedEvent;
        }

        private void EventsManager_ObjectChangedEvent(object sender, ObjectChangedEventArgs e)
        {
            if (e.ObjectChanged is Course)
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

        private string _GroupName;
        public string GroupName
        {
            get
            {
                return _GroupName ?? (_GroupName = Constants.GroupText);
            }
            set
            {
                _GroupName = value;
                OnPropertyChanged("GroupName");
            }
        }

        private void AddGroup()
        {
            if (SelectedCourse != null)
            {
                using (var _context = new ApplicationContext())
                {
                    var group = new Group()
                    {
                        Title = GroupName,
                        Course = _context.Courses.First(x => x.ID == SelectedCourse.ID)
                    };
                    _context.Groups.Add(group);
                    _context.SaveChanges();
                    EventsManager.RaiseObjectChangedEvent(group, ChangeType.Added);
                }
            }
        }

        private ICommand _AddGroupCommand;
        public ICommand AddGroupCommand =>
            _AddGroupCommand ??
            (_AddGroupCommand = new RelayCommand(
                (obj) =>
                {
                    AddGroup();
                },
                (obj) =>
                {
                    return true;
                }));
    }
}
