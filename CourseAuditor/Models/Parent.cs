using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseAuditor.Models
{
    public class Parent : ObservableObject
    {
        private string _FirstName;
        private string _SecondName;
        private string _Patronymic;
        private string _Phone;
        private ICollection<PersonParent> _Children;

        public override string ToString()
        {
            return $"{FirstName} {SecondName} {Patronymic}  {Phone}";
        }
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

        public virtual ICollection<PersonParent> Children
        {
            get
            {
                return _Children;
            }
            set
            {
                _Children = value;
                OnPropertyChanged("Children");
            }
        }

        public override string ToString()
        {
            return $"{SecondName} {FirstName} {Patronymic} ({Phone})";
        }

        [NotMapped]
        public string FullName
        {
            get
            {
                return _FirstName + " " + _SecondName;
            }
            set
            {
                string[] parts = value.Split(' ');
                _FirstName = parts[0];
                _SecondName = parts[1];
                if (parts.Length > 2)
                {
                    _Patronymic = parts[2];
                }
            }
        }

    }
}
