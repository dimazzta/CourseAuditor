using CourseAuditor.DAL;
using CourseAuditor.Helpers;
using CourseAuditor.Models;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CourseAuditor.ViewModels
{
    class CertificateStudentPageVM: BaseVM, IPageVM
    {

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
                Student  = _context.Students.Include(x => x.Module)
                      .Include(x => x.Module.Group)
                      .Include(x => x.Module.Group.Course)
                      .Include(x => x.Person)
                      .FirstOrDefault(x => x.ID == student.ID);
            }
        }

        void Print()
        {
            string templateFileName = "";
            string savePath = "";
            CommonOpenFileDialog templateDialog = new CommonOpenFileDialog("Выберите шаблон для печати");
            if (templateDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                templateFileName = templateDialog.FileName;
            }
            else
            {
                return;
            }

            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                savePath = dialog.FileName;
            }
            else
            {
                return;
            }

            var cf = new CreaterCertificates(templateFileName, savePath, new List<Student>() { Student });
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


        public string PageTitle => "Печать сертификатов для студента";
    }
}
