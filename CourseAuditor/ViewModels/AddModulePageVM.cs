using CourseAuditor.DAL;
using CourseAuditor.Helpers;
using CourseAuditor.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

namespace CourseAuditor.ViewModels
{
    public class AddModulePageVM : BaseVM, IPageVM
    {
        public AddModulePageVM(Group selectedGroup = null)
        {
            Students = new ObservableCollection<CheckedListItem<Student>>();
            using (var _context = new ApplicationContext())
            {
                Groups = new ObservableCollection<Group>(_context.Groups.Include(x => x.Modules));
                var students = _context.Students.Include(x => x.Person).Include(x => x.Module.Group).ToList();
                foreach(var student in students)
                {
                    Students.Add(new CheckedListItem<Student>(student));
                }
            }
            if (selectedGroup != null)
            {
                SelectedGroup = selectedGroup;
            }
            else
            {
                SelectedGroup = Groups.FirstOrDefault();
            }

            CalculateDateBounds();

            EventsManager.ObjectChangedEvent += (s, e) =>
            {
                if (e.ObjectChanged is Group)
                {
                    using (var _context = new ApplicationContext())
                    {
                        Groups = new ObservableCollection<Group>(_context.Groups);
                    }
                    if (SelectedGroup == null)
                    {
                        SelectedGroup = Groups.FirstOrDefault();
                    }
                }
            };
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

        private void CalculateDateBounds()
        {
            DateStart = DateTime.Now;
            DateEnd = DateStart.AddMonths(Constants.DefaultModuleLengthMonth);
        }

        private DateTime _DateStart;
        public DateTime DateStart
        {
            get
            {
                return _DateStart;
            }
            set
            {
                _DateStart = value;
                OnPropertyChanged("DateStart");
            }
        }

        private DateTime _DateEnd;
        public DateTime DateEnd
        {
            get
            {
                return _DateEnd;
            }
            set
            {
                _DateEnd = value;
                OnPropertyChanged("DateEnd");
            }
        }

        private int _ModuleNumber;
        public int ModuleNumber
        {
            get
            {
                return _ModuleNumber;
            }
            set
            {
                _ModuleNumber = value;
                OnPropertyChanged("ModuleNumber");
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

        private Group _SelectedGroup;
        public Group SelectedGroup
        {
            get
            {
                return _SelectedGroup;
            }
            set
            {
                _SelectedGroup = value;
                OnPropertyChanged("SelectedGroup");
                if (_SelectedGroup != null)
                {
                    if (_SelectedGroup.Modules.Count > 0)
                    {
                        ModuleNumber = _SelectedGroup.Modules.OrderBy(x => x.Number).Select(x => x.Number).Last() + 1;
                    }
                    else ModuleNumber = 1;
                }
                
            }
        }

    }
}
