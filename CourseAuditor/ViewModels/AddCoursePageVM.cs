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

        private double? _CoursePrice;
        public double? CoursePrice
        {
            get
            {
                return _CoursePrice ?? (_CoursePrice = Constants.LessonPrice);
            }
            set
            {
                _CoursePrice = Convert.ToDouble(value);
                OnPropertyChanged("CoursePrice");
            }
        }

        private int? _CourseLessonsCount;
        public int? CourseLessonsCount
        {
            get
            {
                return _CourseLessonsCount ?? (_CourseLessonsCount = Constants.LessonsCount);
            }
            set
            {
                _CourseLessonsCount = Convert.ToInt32(value);
                OnPropertyChanged("CourseLessonsCount");
            }
        }



        private void AddCourse()
        {
            var SelectedCourse = new Course
            {
                Name = _CourseName,
                LessonPrice = _CoursePrice.Value,
                LessonCount = _CourseLessonsCount.Value
            };
            using (var _context = new ApplicationContext())
            {
                _context.Courses.Add(SelectedCourse);
                _context.SaveChanges();
            }
            EventsManager.RaiseObjectChangedEvent(SelectedCourse, ChangeType.Added);
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
                    return CourseName != null && CourseName != null && CourseLessonsCount != null;
                }
        ));
    }
}
