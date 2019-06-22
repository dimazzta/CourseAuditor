using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CourseAuditor.DAL;
using CourseAuditor.Helpers;
using CourseAuditor.Models;
using CourseAuditor.Views;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace CourseAuditor.ViewModels
{
    public class CertificateModulePageVM : BaseVM, IPageVM
    {
        private bool _SelectAll;
        public bool SelectAll
        {
            get
            {
                return _SelectAll;
            }
            set
            {
                _SelectAll = value;
                foreach(var st in Students)
                {
                    st.IsChecked = value;
                }
                OnPropertyChanged("SelectAll");
            }
        }

        private ObservableCollection<CheckedListItem<Student>> _Students;
        public ObservableCollection<CheckedListItem<Student>> Students
        {
            get
            {
                return _Students;
            }
            set
            {
                _Students = value;
                OnPropertyChanged("Students");
            }
        }

        public CertificateModulePageVM(Module module)
        {
            using (ApplicationContext _context = new ApplicationContext())
            {
                Students = new ObservableCollection<CheckedListItem<Student>>();
                foreach (var item in module.Students)
                {
                    Students.Add(new CheckedListItem<Student>(item));
                }        
            }            
        }

        private void Print()
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

           
            var cf = new CreaterCertificates(templateFileName, savePath, 
                Students
                .Where(x => x.IsChecked)
                .Select(x => x.Item)
                .ToList());
        
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

        public string PageTitle => "Печать сертификатов для модуля";
    }
}
