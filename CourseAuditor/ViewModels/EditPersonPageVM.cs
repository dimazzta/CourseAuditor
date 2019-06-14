using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using CourseAuditor.DAL;
using CourseAuditor.Helpers;
using CourseAuditor.Models;
using System.Data.Entity;

namespace CourseAuditor.ViewModels
{
    class EditPersonPageVM : BaseVM, IPageVM
    {
        private string _SecondName;
        private string _Patronymic;
        private string _Phone;



        private ObservableCollection<Student> _Students;
        public ObservableCollection<Student> Students
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

        

        private Person _Person;
        public Person Person
        {
            get
            {

                return _Person;
            }
            set
            {
                _Person = value;
                OnPropertyChanged("Person");
            }
        }

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

        private ObservableCollection<Parent>_Parents;
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

        private ObservableCollection<Group> _Groups;
        public ObservableCollection<Group> Groups
        {
            get
            {
                return _Groups;
            }
            set
            {
                _Groups = value;
                OnPropertyChanged("Groups");
            }
        }

        public EditPersonPageVM(Student student)
        {
            #region вывод в текстбоксы
            Person = new Person();
            Parents = new ObservableCollection<Parent>();
            using(ApplicationContext _context = new ApplicationContext())
            {
                var personPer = _context.PersonParents.Where(x => x.Person_ID == student.Person_ID);
                var parents = personPer.Select(x => x.Parent);
                foreach (var item in parents)
                {
                    Parents.Add(item);
                }
                var person = personPer.FirstOrDefault();
                var selectPerson = _context.Persons.Where(x => x.ID == person.Person_ID).FirstOrDefault();
                Person = selectPerson;
                Phone = selectPerson.Phone;
                Patronymic = selectPerson.Patronymic;
                FirstName = selectPerson.FirstName;
                SecondName = selectPerson.SecondName;
                #endregion

                //Надо кароч какта вывести данные в каком модуле, группе, и курсе челикс в нижний листбокс
                Students = new ObservableCollection<Student>(_context.Students.Where(x => x.Person_ID == selectPerson.ID));
            }
        }

        private void EditPerson()
        {

            Person.FirstName = FirstName;
            Person.SecondName = SecondName;
            Person.Patronymic = Patronymic;
            Person.Phone = Phone;
            using (var _context = new ApplicationContext())
            {
                var person = _context.Persons.FirstOrDefault(x => x.ID == Person.ID);
                _context.Entry(person).CurrentValues.SetValues(Person);
                _context.SaveChanges();
            }
            EventsManager.RaiseObjectChangedEvent(Person);
        }
        private ICommand _EditPersonCommand;
        public ICommand EditPersonCommand =>
            _EditPersonCommand ??
            (_EditPersonCommand = new RelayCommand(
                (obj) =>
                {
                    EditPerson();
                }
                ));


    }
}
