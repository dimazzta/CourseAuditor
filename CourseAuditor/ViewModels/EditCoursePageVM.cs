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
    public class EditCoursePageVM : BaseVM, IPageVM
    {
        public EditCoursePageVM(Course SelectedCourse)
        {
            this.SelectedCourse = SelectedCourse;
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



        private void AddCourse()
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

        private RelayCommand _AddCourseCommand;
        public RelayCommand AddCourseCommand =>
            _AddCourseCommand ??
            (_AddCourseCommand = new RelayCommand(
                (obj) =>
                {
                    AddCourse();
                },
                (obj) =>
                {
                    return CourseName != null && CoursePrice != 0 && CourseLessonsCount != 0;
                }
        ));
    }
}
