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

namespace CourseAuditor.ViewModels
{
    public class EditGroupPageVM : BaseVM, IPageVM
    { 
        public EditGroupPageVM(Group selectedGroup = null)
        {
            using (var _context = new ApplicationContext())
            { 
                Courses = new ObservableCollection<Course>(_context.Courses.Include(x => x.Groups.Select(t => t.Modules)));
            }
            if (selectedGroup != null)
            {
                SelectedGroup = selectedGroup;
                SelectedCourse = selectedGroup.Course;
            }
            else
            {
                SelectedCourse = Courses.FirstOrDefault();
            }

            EventsManager.ObjectChangedEvent += EventsManager_ObjectChangedEvent;
        }

        private void EventsManager_ObjectChangedEvent(object sender, ObjectChangedEventArgs e)
        {
            if (e.ObjectChanged is Group || e.ObjectChanged is Course)
            {
                int? id = SelectedGroup?.ID;
                using (var _context = new ApplicationContext())
                {
                    Courses = new ObservableCollection<Course>(_context.Courses.Include(x => x.Groups.Select(t => t.Modules)));
                    if (id != null)
                    {
                        SelectedGroup = _context.Groups.FirstOrDefault(x => x.ID == id);
                        SelectedCourse = SelectedGroup?.Course;
                        if (SelectedCourse != null)
                            Groups = new ObservableCollection<Group>(SelectedCourse.Groups);
                        else
                            SelectedCourse = Courses.FirstOrDefault();
                    }
                    else
                    {
                        SelectedCourse = Courses.FirstOrDefault();
                    }
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
                GroupName = _SelectedGroup?.Title;
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
                if (_SelectedCourse != null)
                {
                    Groups = new ObservableCollection<Group>(_SelectedCourse.Groups);
                    if (SelectedGroup == null)
                        SelectedGroup = Groups?.FirstOrDefault();
                }
                else
                {
                    Groups = new ObservableCollection<Group>();
                }
                OnPropertyChanged("SelectedCourse");
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

        private ObservableCollection<Course> _Courses;
        public ObservableCollection<Course> Courses
        {
            get
            {
                return _Courses;
            }
            set
            {
                _Courses = value;
                OnPropertyChanged("Courses");
            }
        }

        private string _GroupName;
        public string GroupName
        {
            get
            {
                return _GroupName ?? (_GroupName = Constants.GroupText);
            }
            set
            {
                _GroupName = value;
                OnPropertyChanged("GroupName");
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
                EventsManager.RaiseObjectChangedEvent(SelectedGroup);
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
                        group.Title = GroupName;
                        _context.SaveChanges();
                    }
                }
                EventsManager.RaiseObjectChangedEvent(SelectedGroup);
            }
        }

        private RelayCommand _EditGroupCommand;
        public RelayCommand EditGroupCommand =>
            _EditGroupCommand ??
            (_EditGroupCommand = new RelayCommand(
                (obj) =>
                {
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
    }
}
