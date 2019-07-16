using CourseAuditor.DAL;
using CourseAuditor.Helpers;
using CourseAuditor.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CourseAuditor.ViewModels
{
    public class EditCoursePageVM : BaseVM, IPageVM, IEditPageVM
    {
        public EditCoursePageVM() { }
        public EditCoursePageVM(ICommand goBack, Course SelectedCourse = null)
        {
            GoBack = goBack;
            this.SelectedCourse = SelectedCourse;
            EventsManager.ObjectChangedEvent += EventsManager_ObjectChangedEvent;
        }

        private void EventsManager_ObjectChangedEvent(object sender, ObjectChangedEventArgs e)
        {
            if(e.ObjectChanged is Course && e.Type == ChangeType.Deleted)
            {
                if (e.ObjectChanged is Course && (e.ObjectChanged as Course).ID == SelectedCourse?.ID || SelectedCourse == null)
                {
                    object o = new object();
                    GoBack.Execute(o);
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

        private string _CoursePrice;
        public string CoursePrice
        {
            get
            {
                return _CoursePrice;
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
                return _CourseLessonsCount;
            }
            set
            {
                _CourseLessonsCount = value;
                OnPropertyChanged("CourseLessonsCount");
            }
        }

        private void WriteInfo()
        {
            if(SelectedCourse != null)
            {
                CourseName = SelectedCourse.Name;
                CoursePrice = SelectedCourse.LessonPrice.ToString();
                CourseLessonsCount = SelectedCourse.LessonCount.ToString();
            }
        }

        public static void DeleteCourse(Course SelectedCourse)
        {
            if (SelectedCourse != null)
            {
                if (SelectedCourse.Groups.Count != 0)
                {
                    var f = MessageBox.Show("Вы уверены?", "Удаление курса", MessageBoxButton.YesNo);
                    if (f == MessageBoxResult.No)
                    {
                        return;
                    }
                }
                using (var _context = new ApplicationContext())
                {
                    var deleted = _context.Courses.First(x => x.ID == SelectedCourse.ID);
                    if (deleted != null)
                    {
                        _context.Courses.Remove(deleted);
                        _context.SaveChanges();
                    }
                }
                AppState.I.SelectedContextCourse = null;
                EventsManager.RaiseObjectChangedEvent(SelectedCourse, ChangeType.Deleted);
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

        private void EditCourse()
        {
            if (SelectedCourse != null)
            {
                SelectedCourse.Name = CourseName;
                SelectedCourse.LessonPrice = double.Parse(CoursePrice);
                SelectedCourse.LessonCount = int.Parse(CourseLessonsCount);
                using (var _context = new ApplicationContext())
                {
                    var course = _context.Courses.FirstOrDefault(x => x.ID == SelectedCourse.ID);
                    if (course != null)
                    {
                        _context.Entry(course).CurrentValues.SetValues(SelectedCourse);
                        _context.SaveChanges();
                    }
                }
                EventsManager.RaiseObjectChangedEvent(SelectedCourse, ChangeType.Updated);
            }
        }

        private RelayCommand _EditCourseCommand;
        public RelayCommand EditCourseCommand =>
            _EditCourseCommand ??
            (_EditCourseCommand = new RelayCommand(
                (obj) =>
                {
                    if (Validate())
                        EditCourse();
                },
                (obj) =>
                {
                    return true;
                }
        ));

        private RelayCommand _DeleteCourseCommand;
        public RelayCommand DeleteCourseCommand =>
            _DeleteCourseCommand ??
            (_DeleteCourseCommand = new RelayCommand(
                (obj) =>
                {
                    DeleteCourse(SelectedCourse);
                },
                (obj) =>
                {
                    return true;
                }
        ));
       
        public ICommand GoBack { get; set; }

        public string PageTitle => "Редактирование курса";
    }
}
