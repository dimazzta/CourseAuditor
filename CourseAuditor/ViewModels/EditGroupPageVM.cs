using CourseAuditor.DAL;
using CourseAuditor.Helpers;
using CourseAuditor.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Windows;
using System.Windows.Input;

namespace CourseAuditor.ViewModels
{
    public class EditGroupPageVM : BaseVM, IPageVM, IEditPageVM
    { 
        public EditGroupPageVM(ICommand goBack, Group selectedGroup = null)
        {
            GoBack = goBack;
            if (selectedGroup != null)
            {
                SelectedGroup = selectedGroup;
                SelectedCourse = selectedGroup.Course;
            }

            EventsManager.ObjectChangedEvent += EventsManager_ObjectChangedEvent;
        }

        private void EventsManager_ObjectChangedEvent(object sender, ObjectChangedEventArgs e)
        {
            if ((e.ObjectChanged is Course || (e.ObjectChanged is Group) && e.Type == ChangeType.Deleted)){
                if (e.ObjectChanged is Course && (e.ObjectChanged as Course).ID == SelectedCourse?.ID
                 || e.ObjectChanged is Group && (e.ObjectChanged as Group).ID == SelectedGroup?.ID
                 || SelectedGroup == null)
                {
                    object o = new object();
                    GoBack.Execute(o);
                }
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
            }
        }

        private Course _SelectedCourse;
        public Course SelectedCourse
        {
            get
            {
                return _SelectedCourse;
            }
            set
            {
                _SelectedCourse = value;
                OnPropertyChanged("SelectedCourse");
            }
        }


        public static void DeleteGroup(Group SelectedGroup)
        {
            if (SelectedGroup != null)
            {
                if (SelectedGroup.Modules.Count != 0)
                {
                    var f = MessageBox.Show("Группа не пуста. Вы уверены, что хотите удалить ее?", "Группа не пуста", MessageBoxButton.YesNo);
                    if (f == MessageBoxResult.No)
                    {
                        return;
                    }
                }
                using (var _context = new ApplicationContext())
                {
                    var deleted = _context.Groups.First(x => x.ID == SelectedGroup.ID);
                    if (deleted != null)
                    {
                        _context.Groups.Remove(deleted);
                        _context.SaveChanges();
                    }
                }
                AppState.I.SelectedContextGroup = null;
                EventsManager.RaiseObjectChangedEvent(SelectedGroup, ChangeType.Deleted);
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
            StringBuilder err = new StringBuilder();

            if (string.IsNullOrEmpty(SelectedGroup.Title))
            {
                err.Append("*Название группы не может быть пустым. \n");
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

        private void UpdateGroup()
        {
            if (SelectedGroup != null)
            {
                using (var _context = new ApplicationContext())
                {
                    var group = _context.Groups.FirstOrDefault(x => x.ID == SelectedGroup.ID);
                    if (group != null)
                    {
                        group.Title = SelectedGroup.Title;
                        _context.SaveChanges();
                    }
                }
                EventsManager.RaiseObjectChangedEvent(SelectedGroup, ChangeType.Updated);
            }
        }

        private RelayCommand _EditGroupCommand;
        public RelayCommand EditGroupCommand =>
            _EditGroupCommand ??
            (_EditGroupCommand = new RelayCommand(
                (obj) =>
                {
                    if (Validate())
                        UpdateGroup();
                },
                (obj) =>
                {
                    return true;
                }
        ));

        private RelayCommand _DeleteGroupCommand;
        public RelayCommand DeleteGroupCommand =>
            _DeleteGroupCommand ??
            (_DeleteGroupCommand = new RelayCommand(
                (obj) =>
                {
                    DeleteGroup(SelectedGroup);
                },
                (obj) =>
                {
                    return true;
                }
        ));

        public ICommand GoBack { get; set; }


        public string PageTitle => "Редактирование группы";
    }
}
