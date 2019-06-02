using CourseAuditor.DAL;
using CourseAuditor.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseAuditor.ViewModels
{
    public class MainVM : BaseVM
    {
        private ApplicationContext _context;
        public ObservableCollection<Course> Courses { get; set; }

        public ObservableCollection<Student> Students { get; set; }
        public Group SelectedGroup { get; set; }

        public MainVM()
        {
            _context = new ApplicationContext();
            Courses = new ObservableCollection<Course>(_context.Courses);
            Students = new ObservableCollection<Student>(_context.Students);
            SelectedGroup = new Group();
        }
    }
}
