using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourseAuditor.DAL;
using CourseAuditor.Helpers;
using CourseAuditor.Models;
using CourseAuditor.Views;

namespace CourseAuditor.ViewModels
{
    public class AddParentVM : BaseVM, IViewVM
    {
        public IPageVM CurrentPageVM { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IView CurrentView { get; set; }

        private ObservableCollection<Parent> _Parents;
        public ObservableCollection<Parent> Parents
        {
            get
            {
                return _Parents;
            }
            set
            {
                _Parents = value;
                OnPropertyChanged("Parents");
            }
        }

        public bool AddNewMode { get; set; }

        private string _FirstName;
        public string FirstName
        {
            get
            {
                return _FirstName;
            }
            set
            {
                _FirstName = value;
                OnPropertyChanged("FirstName");
            }
        }

        private string _SecondName;
        public string SecondName
        {
            get
            {
                return _SecondName;
            }
            set
            {
                _SecondName = value;
                OnPropertyChanged("SecondName");
            }
        }

        private string _Patronymic;
        public string Patronymic
        {
            get
            {
                return _Patronymic;
            }
            set
            {
                _Patronymic = value;
                OnPropertyChanged("Patronymic");
            }
        }

        private string _Phone;
        public string Phone
        {
            get
            {
                return _Phone;
            }
            set
            {
                _Phone = value;
                OnPropertyChanged("Phone");
            }
        }

        private Parent _ComboboxSelectedParent;
        public Parent ComboboxSelectedParent
        {
            get
            {
                return _ComboboxSelectedParent;
            }
            set
            {
                _ComboboxSelectedParent = value;
                OnPropertyChanged("ComboboxSelectedParent");
            }
        }

        private Parent _SelectedParent;
        public Parent SelectedParent
        {
            get
            {
                return _SelectedParent;
            }
            private set
            {
                _SelectedParent = value;
                OnPropertyChanged("SelectedParent");
            }
        }

        private void AddNewParent() // добавление нового (ранее не существующего в базе данных) родителя
        {
            
        }

        private void AddParent()//добавление уже существующего родителя
        {
            if (AddNewMode)
            {
                SelectedParent = new Parent();
                SelectedParent.FirstName = FirstName;
                SelectedParent.SecondName = SecondName;
                SelectedParent.Patronymic = Patronymic;
                SelectedParent.Phone = Phone;
            }
            else
            {
                SelectedParent = ComboboxSelectedParent;
            } 
        }

        private RelayCommand _AddParentCommand;
        public RelayCommand AddParentCommand =>
            _AddParentCommand ??
            (_AddParentCommand = new RelayCommand(
                (obj) =>
                {
                    AddParent();
                },
                (obj) =>
                {
                    return true;
                }
        ));

        public AddParentVM(IView view)
        {
            using (var _context = new ApplicationContext())
            {
                Parents = new ObservableCollection<Parent>(_context.Parents);
            }
            Phone = "+7";
            CurrentView = view;
            CurrentView.DataContext = this;
            CurrentView.Show();
        }
    }
}
