using CourseAuditor.DAL;
using CourseAuditor.Helpers;
using CourseAuditor.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CourseAuditor.ViewModels
{
    class CertificateStudentPageVM:BaseVM, IPageVM
    {


        private string _FullInf;
        public string FullInf
        {
            get
            {
                return _FullInf;
            }
            set
            {
                _FullInf = value;
                OnPropertyChanged("FullInf");
            }
        }


        private Student _Student;
        public Student Student
        {
            get
            {
                return _Student;
            }
            set
            {
                _Student = value;
                OnPropertyChanged("Student");
            }
        }

        public CertificateStudentPageVM(Student student)
        {
            using (ApplicationContext _context = new ApplicationContext())
            {
                var a = _context.Students.Include(x => x.Module)
                      .Include(x => x.Module.Group)
                      .Include(x => x.Module.Group.Course)
                      .Include(x=>x.Person)
                      .FirstOrDefault();
                Student = student;
                FullInf = $"{Student.Person.FullName} выпускник курса {Student.Module.Group.Course.Name}"; 
            }
        }

        void Print()
        {

        }


        private ICommand _PrintCertificate;
        public ICommand PrintCertificate =>
            _PrintCertificate ??
            (_PrintCertificate = new RelayCommand(
                (obj) =>
                {
                    Print();
                }
                ));
    }
}
