using CourseAuditor.DAL;
using CourseAuditor.Models;
using CourseAuditor.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseAuditor.ViewModels
{
    public class EditCourseVM : BaseVM
    {
        private ApplicationContext _context;
        public IFrame CurrentFrame { get; set; }
        private Course _course;

        public Course Course
        {
            get
            {
                return _course;
            }
            set
            {
                _course = value;

            }
        }

        public EditCourseVM(IFrame frame)
        {
            CurrentFrame = frame;
            Course = new Course();

            CurrentFrame = frame;
            CurrentFrame.DataContext = this;
        }

        public EditCourseVM(IFrame frame,Course course)
        {
            CurrentFrame = frame;
            Course = course;

            CurrentFrame = frame;
            CurrentFrame.DataContext = this;
        }

        public void AddCourse(string name, string price, string lessonscount)
        {
            Course course  = new Course();
            course.Name = name;
            course.Price = Convert.ToDouble(price);
            course.LessonsCount = Convert.ToInt32(lessonscount);
            course.Id = 4;
            using (_context = new ApplicationContext()) {
                _context.Courses.Add(course);
                _context.SaveChanges();
            }
        }
    }
}
