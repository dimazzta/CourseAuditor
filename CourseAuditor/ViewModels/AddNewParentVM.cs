using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourseAuditor.Helpers;
using CourseAuditor.Models;
using CourseAuditor.Views;

namespace CourseAuditor.ViewModels
{
    public class AddNewParentVM : BaseVM, IViewVM
    {
        public IPageVM CurrentPageVM { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IView CurrentView { get; set; }

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

        private void AddNewParent()
        {
            SelectedParent = new Parent();
            SelectedParent.FirstName = FirstName;
            SelectedParent.SecondName = SecondName;
            SelectedParent.Patronymic = Patronymic;
            SelectedParent.Phone = Phone;
        }

        private RelayCommand _AddNewParentCommand;
        public RelayCommand AddNewParentCommand =>
            _AddNewParentCommand ??
            (_AddNewParentCommand = new RelayCommand(
                (obj) =>
                {
                    AddNewParent();
                },
                (obj) =>
                {
                    return true;
                }
        ));

        public AddNewParentVM(IView view)
        {
            Phone = "+7";
            CurrentView = view;
            CurrentView.DataContext = this;
            CurrentView.Show();
        }
    }
}
