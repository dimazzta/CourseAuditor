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

namespace CourseAuditor.ViewModels
{
    public class CertificateModulePageVM : BaseVM, IPageVM
    {
        private ObservableCollection<CheckedListItem<Person>> _Persons;
        public ObservableCollection<CheckedListItem<Person>> Persons
        {
            get
            {
                return _Persons;
            }
            set
            {
                _Persons = value;
                OnPropertyChanged("Persons");
            }
        }

        public CertificateModulePageVM(Module module)
        {
            using (ApplicationContext _context = new ApplicationContext())
            {
                Persons = new ObservableCollection<CheckedListItem<Person>>();
                foreach (var item in module.Students.Select(x => x.Person))
                {
                    Persons.Add(new CheckedListItem<Person>(item));
                }        
            }            
        }

        private void Print()
        {

        }

        private ICommand _PrintCertificate;
        public ICommand PrintCertificate =>
            _PrintCertificate ??
            (_PrintCertificate = new RelayCommand(
                (obj) =>
                {
                   // AddModule();
                }
                ));







    }
}
