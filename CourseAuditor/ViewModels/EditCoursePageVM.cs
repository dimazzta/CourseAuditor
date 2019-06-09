using CourseAuditor.DAL;
using CourseAuditor.Helpers;
using CourseAuditor.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CourseAuditor.ViewModels
{
    public class EditCoursePageVM : BaseVM, IPageVM
    {
        public EditCoursePageVM(Course SelectedCourse = null)
        {
            using (var _context = new ApplicationContext())
            {
                Courses = new ObservableCollection<Course>(_context.Courses.Include(x => x.Groups));
            }
            if (SelectedCourse == null)
            {
                this.SelectedCourse = Courses.First();
            }
            else
            {
                this.SelectedCourse = SelectedCourse;
            }
            EventsManager.ObjectChangedEvent += EventsManager_ObjectChangedEvent;
        }

        private void EventsManager_ObjectChangedEvent(object sender, ObjectChangedEventArgs e)
        {
            if(e.ObjectChanged is Course)
            {
                int id = SelectedCourse.ID;
                using (var _context = new ApplicationContext())
                {
                    Courses = new ObservableCollection<Course>(_context.Courses.Include(x => x.Groups));
                }
                if (!Courses.Any(x => x.ID == id))
                    SelectedCourse = Courses.First();
                else
                    SelectedCourse = Courses.First(x => x.ID == id);
                    
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
                OnPropertyChanged("SelectedCourse");
                WriteInfo();
            }
        }

        private string _CourseName;
        public string CourseName
        {
            get
            {
                return _CourseName;
            }
            set
            {
                _CourseName = value;
                OnPropertyChanged("CourseName");
            }
        }

        private double _CoursePrice;
        public double CoursePrice
        {
            get
            {
                return _CoursePrice;
            }
            set
            {
                _CoursePrice = Convert.ToDouble(value);
                OnPropertyChanged("CoursePrice");
            }
        }

        private int _CourseLessonsCount;
        public int CourseLessonsCount
        {
            get
            {
                return _CourseLessonsCount;
            }
            set
            {
                _CourseLessonsCount = Convert.ToInt32(value);
                OnPropertyChanged("CourseLessonsCount");
            }
        }

        private void WriteInfo()
        {
            if(SelectedCourse != null)
            {
                CourseName = SelectedCourse.Name;
                CoursePrice = SelectedCourse.Price;
                CourseLessonsCount = SelectedCourse.LessonsCount;
            }
        }

        private void DeleteCourse()
        {
            if(SelectedCourse.Groups.Count != 0)
            {
                var f = MessageBox.Show("Вы уверены?","Удаление курса",MessageBoxButton.YesNo);
                if (f == MessageBoxResult.No)
                {
                    return;
                }
            }
            using (var _context = new ApplicationContext())
            {
                var deleted = _context.Courses.First(x=>x.ID==SelectedCourse.ID);
                _context.Entry(deleted).State = EntityState.Deleted;
                _context.SaveChanges();
            }
            EventsManager.RaiseObjectChangedEvent(SelectedCourse);
        }

        private void EditCourse()
        {
            SelectedCourse.Name = CourseName;
            SelectedCourse.Price = CoursePrice;
            SelectedCourse.LessonsCount = CourseLessonsCount;
            using (var _context = new ApplicationContext())
            {
                var course = _context.Courses.First(x => x.ID == SelectedCourse.ID);
                _context.Entry(course).CurrentValues.SetValues(SelectedCourse);
                _context.SaveChanges();
            }
            EventsManager.RaiseObjectChangedEvent(SelectedCourse);
        }

        private RelayCommand _EditCourseCommand;
        public RelayCommand EditCourseCommand =>
            _EditCourseCommand ??
            (_EditCourseCommand = new RelayCommand(
                (obj) =>
                {
                    EditCourse();
                },
                (obj) =>
                {
                    return CourseName != null && CoursePrice != 0 && CourseLessonsCount != 0;
                }
        ));

        private RelayCommand _DeleteCourseCommand;
        public RelayCommand DeleteCourseCommand =>
            _DeleteCourseCommand ??
            (_DeleteCourseCommand = new RelayCommand(
                (obj) =>
                {
                    DeleteCourse();
                },
                (obj) =>
                {
                    return CourseName != null && CoursePrice != 0 && CourseLessonsCount != 0;
                }
        ));
    }
}
