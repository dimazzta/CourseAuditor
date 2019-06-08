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
        // Список доступных страниц в данной View
        private IPageVM _JournalPage;
        public IPageVM JournalPage
        {
            get
            {
                if (_JournalPage == null)
                    _JournalPage = new JournalPageVM();
                return _JournalPage;
            }
            set
            {
                _JournalPage = value;
                OnPropertyChanged("JournalPage");
            }
        }

        private IPageVM _AddCourseVM;
        public IPageVM AddCourseVM
        {
            get
            {
                if (_AddCourseVM == null)
                    _AddCourseVM = new AddCoursePageVM();
                return _AddCourseVM;
            }
            set
            {
                _AddCourseVM = value;
                OnPropertyChanged("AddCourseVM");
            }
        }

        // Текущая страница
        private IPageVM _CurrentPageVM;
        public IPageVM CurrentPageVM
        {
            get
            {
                return _CurrentPageVM;
            }
            set
            {
                _CurrentPageVM = value;
                OnPropertyChanged("CurrentPageVM");
            }
        }


        // Команда для смены страницы. Параметр команды - IPageVM, его передаем из xaml 
        // за счет того что указан DataTemplate. Таким образом реализуется главный принцип
        // MVVM - VM ничего не знают о V
        private ICommand _ChangePage;
        public ICommand ChangePage =>
            _ChangePage ??
            (_ChangePage = new RelayCommand(
                (obj) =>
                {
                    CurrentPageVM = (obj as IPageVM);
                }
             ));

        //UI View
        public IView CurrentView { get; set; }


        //Логика самого View
        public ObservableCollection<Course> Courses { get; set; }

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
                Courses = new ObservableCollection<Course>(_context.Courses.Include(x => x.Groups.Select(t => t.Modules)));
            }

            AppState.I.PropertyChanged += StatePropertyChanged;

            CurrentView = view;
            CurrentView.DataContext = this;
            CurrentPageVM = JournalPage;
            CurrentView.Show();
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
