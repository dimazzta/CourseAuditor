using CourseAuditor.DAL;
using CourseAuditor.Helpers;
using CourseAuditor.Models;
using CourseAuditor.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Data.Entity;

namespace CourseAuditor.ViewModels
{
    public class MainVM : BaseVM, IViewVM
    {
        private IPageVM _CurrentPageVM;
        public IPageVM CurrentPageVM
        {
            get => _CurrentPageVM;
            set
            {
                _CurrentPageVM = value;
                OnPropertyChanged("CurrentPageVM");
            }
        }

        // Команда для смены страницы. Под кажду страницу своя команда. В команде явно создаем новый экземпляр VM.
        private ICommand _JournalPage;
        public ICommand JournalPage =>
            _JournalPage ??
            (_JournalPage = new RelayCommand(
                (obj) =>
                {
                    CurrentPageVM = new JournalPageVM();
                }
             ));

        private ICommand _AddCoursePage;
        public ICommand AddCoursePage =>
            _AddCoursePage ??
            (_AddCoursePage = new RelayCommand(
                (obj) =>
                {
                    CurrentPageVM = new AddCoursePageVM();
                }
             ));


        private ICommand _AddGroupPage;
        public ICommand AddGroupPage =>
            _AddGroupPage ??
            (_AddGroupPage = new RelayCommand(
                (obj) =>
                {
                    CurrentPageVM = new AddGroupPageVM(AppState.I.SelectedContextCourse);
                }
             ));

        private ICommand _AddModulePage;
        public ICommand AddModulePage =>
            _AddModulePage ??
            (_AddModulePage = new RelayCommand(
                (obj) =>
                {
                    CurrentPageVM = new AddModulePageVM(AppState.I.SelectedContextGroup);
                }
             ));

        private ICommand _EditCoursePage;
        public ICommand EditCoursePage =>
            _EditCoursePage ??
            (_EditCoursePage = new RelayCommand(
                (obj) =>
                {
                    CurrentPageVM = new EditCoursePageVM(AppState.I.SelectedContextCourse);
                }
             ));

        private ICommand _EditGroupPage;
        public ICommand EditGroupPage =>
            _EditGroupPage ??
            (_EditGroupPage = new RelayCommand(
                (obj) =>
                {
                    CurrentPageVM = new EditGroupPageVM(AppState.I.SelectedContextGroup);
                }
             ));

        private ICommand _DeleteCourse;
        public ICommand DeleteCourse =>
            _DeleteCourse ??
            (_DeleteCourse = new RelayCommand(
                (obj) =>
                {
                    EditCoursePageVM.DeleteCourse(AppState.I.SelectedContextCourse);
                }
                ));

        private ICommand _DeleteGroup;
        public ICommand DeleteGroup =>
            _DeleteGroup ??
            (_DeleteGroup = new RelayCommand(
                (obj) =>
                {
                    EditGroupPageVM.DeleteGroup(AppState.I.SelectedContextGroup);
                }
                ));

        private ICommand _AddPaymentWindow;
        public ICommand AddPaymentWindow =>
            _AddPaymentWindow ??
            (_AddPaymentWindow = new RelayCommand(
                (obj) =>
                {
                    new PaymentVM(new PaymentWindow(),AppState.I.SelectedContextStudent);
                }
             ));
        //TODO исп
        private ICommand _AddReturnPay;
        public ICommand AddReturnPay =>
            _AddReturnPay ??
            (_AddReturnPay = new RelayCommand(
                (obj) =>
                {
                    new ReturnPaymentVM(new AddReturnPay(), AppState.I.SelectedContextStudent);
                }
             ));

        //UI View
        public IView CurrentView { get; set; }


        //Логика самого View
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
                SelectedModule = _SelectedGroup.LastModule;
                if (_SelectedGroup != AppState.I.SelectedGroup)
                    AppState.I.SelectedGroup = _SelectedGroup;

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
                if (value != _SelectedModule)
                {
                    _SelectedModule = value;

                    // В сеттере меняем глобальное состояние (если это предсмотрено логикой)
                    if (_SelectedModule != AppState.I.SelectedModule)
                        AppState.I.SelectedModule = _SelectedModule;

                    OnPropertyChanged("SelectedModule");
                }
            }
        }

        


        #region Contructors
        public MainVM(IView view)
        {
            using(var _context = new ApplicationContext())
            {
                Courses = new ObservableCollection<Course>(_context.Courses
                                                           .Include(x => x.Groups
                                                           .Select(t => t.Modules
                                                           .Select(m => m.Students
                                                           .Select(q => q.Person)))));
            }
            EventsManager.ObjectChangedEvent += EventsManager_ObjectChangedEvent;
            AppState.I.PropertyChanged += StatePropertyChanged;

            CurrentView = view;
            CurrentView.DataContext = this;
            CurrentPageVM = new JournalPageVM();
            CurrentView.Show();
        }

        private void EventsManager_ObjectChangedEvent(object sender, ObjectChangedEventArgs e)
        {
            if (e.ObjectChanged is Group || e.ObjectChanged is Course || e.ObjectChanged is Student || e.ObjectChanged is Module)
            {
                using (var _context = new ApplicationContext())
                {
                    Courses = new ObservableCollection<Course>(_context.Courses
                                                           .Include(x => x.Groups
                                                           .Select(t => t.Modules
                                                           .Select(m => m.Students
                                                           .Select(q => q.Person)))));
                }
            }
        }

        private void StatePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "SelectedModule":
                    this.SelectedModule = AppState.I.SelectedModule;
                    break;
                case "SelectedGroup":
                    this.SelectedGroup = AppState.I.SelectedGroup;
                    break;
            }
        }

        #endregion
    }
}
