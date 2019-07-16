using CourseAuditor.DAL;
using CourseAuditor.Helpers;
using CourseAuditor.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CourseAuditor.ViewModels
{
    public class AddCoursePageVM : BaseVM, IPageVM, IValidatable
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

        private string _CoursePrice;
        public string CoursePrice
        {
            get
            {
                return _CoursePrice ?? (_CoursePrice = Constants.LessonPrice.ToString());
            }
            set
            {
                _CoursePrice = value;
                OnPropertyChanged("CoursePrice");
            }
        }

        private string _CourseLessonsCount;
        public string CourseLessonsCount
        {
            get
            {
                return _CourseLessonsCount ?? (_CourseLessonsCount = Constants.LessonsCount.ToString());
            }
            set
            {
                _CourseLessonsCount = value;
                OnPropertyChanged("CourseLessonsCount");
            }
        }



        private void AddCourse()
        {
            var SelectedCourse = new Course
            {
                Name = _CourseName,
                LessonPrice = double.Parse(_CoursePrice),
                LessonCount = int.Parse(_CourseLessonsCount)
            };
            using (var _context = new ApplicationContext())
            {
                _context.Courses.Add(SelectedCourse);
                _context.SaveChanges();
            }
            EventsManager.RaiseObjectChangedEvent(SelectedCourse, ChangeType.Added);
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
           
            if (string.IsNullOrEmpty(CourseName))
            {
                err.Append("*Название курса не может быть пустым. \n");
            }
            int n;
            if (!Int32.TryParse(CourseLessonsCount, out n))
            {
                err.Append("*Количество занятий должно быть числом. \n");
            }
            else if (n <= 0)
            {
                err.Append("*Количество занятий должно быть больше нуля. \n");
            }

            double d;
            if (!Double.TryParse(CoursePrice, out d))
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


        private RelayCommand _AddCourseCommand;
        public RelayCommand AddCourseCommand =>
            _AddCourseCommand ??
            (_AddCourseCommand = new RelayCommand(
                (obj) =>
                {
                    if (Validate())
                    {
                        AddCourse();
                    }
                },
                (obj) =>
                {
                    return true;
                }
        ));

        public string PageTitle => "Добавление курса";
        
    }
}
