using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseAuditor.Models
{
    public class Person : ObservableObject
    {
        private string _FirstName;
        private string _SecondName;
        private string _Patronymic;
        private string _Phone;
        private ICollection<PersonParent> _Parents;
        private ICollection<MedicalDoc> _MedicalDocs;
        private ICollection<Student> _Students;

        public virtual ICollection<Student> Students
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

      

        public virtual ICollection<MedicalDoc> MedicalDocs
        {
            get
            {
                return _MedicalDocs;
            }
            set
            {
                _MedicalDocs = value;
                OnPropertyChanged("MedicalDocs");
            }
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

        public virtual ICollection<PersonParent> Parents
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

        public override string ToString()
        {
            return $"{SecondName} {FirstName} {Patronymic}";
        }
    }
}
