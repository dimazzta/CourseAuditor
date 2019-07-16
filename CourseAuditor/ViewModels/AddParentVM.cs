using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CourseAuditor.DAL;
using CourseAuditor.Helpers;
using CourseAuditor.Models;
using CourseAuditor.ViewModels.Dialogs;
using CourseAuditor.Views;

namespace CourseAuditor.ViewModels
{
    public class AddParentVM : BaseVM, IDialogRequestClose
    {

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

        private Parent _SelectedParent;
        public Parent SelectedParent
        {
            get
            {
                return _SelectedParent;
            }
            set
            {
                _SelectedParent = value;
                OnPropertyChanged("SelectedParent");
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
            if (AddNewMode)
            {
                StringBuilder err = new StringBuilder();

                if (string.IsNullOrEmpty(FirstName))
                {
                    err.Append("*Имя не может быть пустым. \n");
                }
                if (string.IsNullOrEmpty(SecondName))
                {
                    err.Append("*Фамилия не может быть пустой. \n");
                }
                if (string.IsNullOrEmpty(Phone))
                {
                    err.Append("*Телефон не может быть пустым. \n");
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
            return true;
        }


        public Parent Parent { get; private set; }

        private void AddParent()
        {
            if (AddNewMode)
            {
                Parent = new Parent();
                Parent.FirstName = FirstName;
                Parent.SecondName = SecondName;
                Parent.Patronymic = Patronymic;
                Parent.Phone = Phone;
            }
            else
            {
                Parent = SelectedParent;
            } 
        }

        private ICommand _AddParentCommand;

        public ICommand AddParentCommand =>
            _AddParentCommand ??
            (_AddParentCommand = new RelayCommand(
                (obj) =>
                {
                    if (Validate())
                    {
                        AddParent();
                        CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(true));
                    }
                    
                },
                (obj) =>
                {
                    return true;
                }
        ));

        public AddParentVM()
        {
            AddNewMode = true;
            using (var _context = new ApplicationContext())
            {
                Parents = new ObservableCollection<Parent>(_context.Parents);
            }
            Phone = Constants.DefaultPhoneNumberStart;
        }

        public event EventHandler<DialogCloseRequestedEventArgs> CloseRequested;
    }
}
