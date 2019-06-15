using CourseAuditor.Helpers;
using CourseAuditor.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CourseAuditor.ViewModels
{
    public class AppState : INotifyPropertyChanged
    {
        // Синглтон. Хранит разделяемые данные. На нужные данные подписываемся из страниц / окон
        // Если станет слишком большим, надо будет разбить под каждую IViewVM
        private AppState() {
            EventsManager.ObjectChangedEvent += EventsManager_ObjectChangedEvent;
        }

        private void EventsManager_ObjectChangedEvent(object sender, ObjectChangedEventArgs e)
        {
            if (e.Type == ChangeType.Deleted)
            {
                if(e.ObjectChanged is Course)
                {
                    var course = e.ObjectChanged as Course;
                    if (SelectedGroup?.Course?.ID == course.ID)
                    {
                        SelectedGroup = null;
                    }
                    if (SelectedModule?.Group?.Course?.ID == course.ID)
                    {
                        SelectedModule = null;
                    }
                }
                if (e.ObjectChanged is Group)
                {
                    var group = e.ObjectChanged as Group;
                    if (SelectedGroup?.ID == group.ID)
                    {
                        SelectedGroup = null;
                    }
                    if (SelectedModule?.Group?.ID == group.ID)
                    {
                        SelectedModule = null;
                    }
                }
                if (e.ObjectChanged is Module)
                {
                    var module = e.ObjectChanged as Module;
                    if (SelectedModule?.ID == module.ID)
                    {
                        SelectedModule = null;
                    }
                }
            }
        }

        private static AppState _I;
        public static AppState I => _I ?? (_I = new AppState());

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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

        private Module _SelectedModule;
        public Module SelectedModule
        {
            get
            {
                return _SelectedModule;
            }
            set
            {
                _SelectedModule = value;
                OnPropertyChanged("SelectedModule");
            }
        }

        private Course _SelectedContextCourse;
        public Course SelectedContextCourse
        {
            get
            {
                return _SelectedContextCourse;
            }
            set
            {
                _SelectedContextCourse = value;
                OnPropertyChanged("SelectedContextCourse");
            }
        }

        private Group _SelectedContextGroup;
        public Group SelectedContextGroup
        {
            get
            {
                return _SelectedContextGroup;
            }
            set
            {
                _SelectedContextGroup = value;
                OnPropertyChanged("SelectedContextGroup");
            }
        }

        private Module _SelectedContextModule;
        public Module SelectedContextModule
        {
            get
            {
                return _SelectedContextModule;
            }
            set
            {
                _SelectedContextModule = value;
                OnPropertyChanged("SelectedContextModule");
            }
        }

        private Student _SelectedContextStudent;
        public Student SelectedContextStudent
        {
            get
            {
                return _SelectedContextStudent;
            }
            set
            {
                _SelectedContextStudent = value;
                OnPropertyChanged("SelectedContextStudent");
            }
        }
    }
}
