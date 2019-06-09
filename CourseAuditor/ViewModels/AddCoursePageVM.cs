using CourseAuditor.DAL;
using CourseAuditor.Helpers;
using CourseAuditor.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseAuditor.ViewModels
{
    public class AddCoursePageVM : BaseVM, IPageVM
    {
        public AddCoursePageVM()
        {
            SelectedCourse = new Course();
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
        public string CoursePrice
        {
            get
            {
                return _CoursePrice.ToString();
            }
            set
            {
                _CoursePrice = Convert.ToDouble(value);
                OnPropertyChanged("CoursePrice");
            }
        }

        private int _CourseLessonsCount;
        public string CourseLessonsCount
        {
            get
            {
                return _CourseLessonsCount.ToString();
            }
            set
            {
                _CourseLessonsCount = Convert.ToInt32(value);
                OnPropertyChanged("CourseLessonsCount");
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
        private Course SelectedCourse
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

        private void AddCourse()
        {
            SelectedCourse.Name = _CourseName;
            SelectedCourse.Price = _CoursePrice;
            SelectedCourse.LessonsCount = _CourseLessonsCount;
            using (var _context = new ApplicationContext())
            {
                _context.Courses.Add(SelectedCourse);
                _context.SaveChanges();
            }
        }

        private RelayCommand _AddCourseCommand;
        public RelayCommand AddCourseCommand =>
            _AddCourseCommand ??
            (_AddCourseCommand = new RelayCommand(
                (obj) =>
                {
                    AddCourse();
                }
        ));
    }
}
