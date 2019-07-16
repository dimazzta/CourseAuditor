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
using CourseAuditor.ViewModels.Dialogs;
using CourseAuditor.Filters;

namespace CourseAuditor.ViewModels
{
    public class MainVM : BaseVM, IViewVM
    {
        public MainVM()
        {

        }
        private string _PageTitle;
        public string PageTitle
        {
            get
            {
                return _PageTitle;
            }
            set
            {
                _PageTitle = value;
                OnPropertyChanged("PageTitle");
            }
        }

        private IPageVM _CurrentPageVM;
        public IPageVM CurrentPageVM
        {
            get => _CurrentPageVM;
            set
            {
                _CurrentPageVM = value;
                PageTitle = _CurrentPageVM.PageTitle;
                _CurrentPageVM.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == "PageTitle") PageTitle = _CurrentPageVM.PageTitle;
                };
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
                    CurrentPageVM = new JournalPageVM(EditPersonPage, CertificateStudentPage);
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

        private ICommand _AddStudentPage;
        public ICommand AddStudentPage =>
            _AddStudentPage ??
            (_AddStudentPage = new RelayCommand(
                (obj) =>
                {
                    CurrentPageVM = new AddStudentPageVM(AppState.I.SelectedContextGroup);
                }
             ));

        private ICommand _EditCoursePage;
        public ICommand EditCoursePage =>
            _EditCoursePage ??
            (_EditCoursePage = new RelayCommand(
                (obj) =>
                {
                    CurrentPageVM = new EditCoursePageVM(JournalPage, AppState.I.SelectedContextCourse);
                }
             ));

        private ICommand _EditGroupPage;
        public ICommand EditGroupPage =>
            _EditGroupPage ??
            (_EditGroupPage = new RelayCommand(
                (obj) =>
                {
                    CurrentPageVM = new EditGroupPageVM(JournalPage, AppState.I.SelectedContextGroup);
                }
             ));

        private ICommand _EditModulePage;
        public ICommand EditModulePage =>
            _EditModulePage ??
            (_EditModulePage = new RelayCommand(
                (obj) =>
                {
                    CurrentPageVM = new EditModulePageVM(JournalPage, AppState.I.SelectedContextModule);
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

        private ICommand _DeleteModule;
        public ICommand DeleteModule =>
            _DeleteModule ??
            (_DeleteModule = new RelayCommand(
                (obj) =>
                {
                    EditModulePageVM.DeleteModule(AppState.I.SelectedContextModule);
                }
                ));

        private ICommand _DeleteStudent;
        public ICommand DeleteStudent =>
            _DeleteStudent ??
            (_DeleteStudent = new RelayCommand(
                (obj) =>
                {
                    EditPersonPageVM.DeleteStudent(AppState.I.SelectedContextStudent);
                },
                (obj) =>
                {
                    return AppState.I.SelectedContextStudent?.Module?.IsClosed == 0;
                }
             ));

        private ICommand _AddPaymentWindow;
        public ICommand AddPaymentWindow =>
            _AddPaymentWindow ??
            (_AddPaymentWindow = new RelayCommand(
                (obj) =>
                {
                    var paymentVM = new PaymentVM(AppState.I.SelectedContextStudent);
                    DialogService.I.ShowDialog(paymentVM);
                }
             ));

        private ICommand _AddMedicalDocWindow;
        public ICommand AddMedicalDocWindow =>
            _AddMedicalDocWindow ??
            (_AddMedicalDocWindow = new RelayCommand(
                (obj) =>
                {
                    var medicalVM = new AddMedicalDocVM(AppState.I.SelectedContextStudent);
                    DialogService.I.ShowDialog(medicalVM);
                },
                (obj) =>
                {
                    return AppState.I.SelectedContextStudent?.Module?.IsClosed == 0;
                }
             ));

        private ICommand _AddReturnPay;
        public ICommand AddReturnPay =>
            _AddReturnPay ??
            (_AddReturnPay = new RelayCommand(
                (obj) =>
                {
                    var returnVM = new ReturnPaymentVM(AppState.I.SelectedContextStudent);
                    DialogService.I.ShowDialog(returnVM);
                }
             ));

        private ICommand _EditPersonPage;
        public ICommand EditPersonPage =>
            _EditPersonPage ??
            (_EditPersonPage = new RelayCommand(
                (obj) =>
                {
                    CurrentPageVM = new EditPersonPageVM(JournalPage, AppState.I.SelectedContextStudent);
                }
             ));

        private ICommand _CertificateModulePage;
        public ICommand CertificateModulePage =>
            _CertificateModulePage ??
            (_CertificateModulePage = new RelayCommand(
                (obj) =>
                {
                    CurrentPageVM = new CertificateModulePageVM(AppState.I.SelectedContextModule);
                }
             ));

        private ICommand _CertificateStudentPage;
        public ICommand CertificateStudentPage =>
            _CertificateStudentPage ??
            (_CertificateStudentPage = new RelayCommand(
                (obj) =>
                {
                    CurrentPageVM = new CertificateStudentPageVM(AppState.I.SelectedContextStudent);
                }));

        private ICommand _ExpandCommand;
        public ICommand ExpandCommand =>
            _ExpandCommand ??
            (_ExpandCommand = new RelayCommand(
                (obj) =>
                {
                    ChangeExpandState(true);
                }));

        private ICommand _CollapseCommand;
        public ICommand CollapseCommand =>
            _CollapseCommand ??
            (_CollapseCommand = new RelayCommand(
                (obj) =>
                {
                    ChangeExpandState(false);
                }));

        private string _FilterNameParam = "";
        private ICommand _FilterNameCommand;
        public ICommand FilterNameCommand =>
            _FilterNameCommand ??
            (_FilterNameCommand = new RelayCommand(
                (obj) =>
                {
                    _FilterNameParam = obj as string;
                    Filter();
                }
                ));


        private int? _FilterModuleParam = 1;
        private ICommand _FilterModuleCommand;
        public ICommand FilterModuleCommand =>
            _FilterModuleCommand ??
            (_FilterModuleCommand = new RelayCommand(
                (obj) =>
                {
                    _FilterModuleParam = Convert.ToInt32(obj);
                    Filter();
                }
                ));


        private IFilter<Course> _NameFilter { get; set; }
        private IFilter<Course> _ModuleFilter { get; set; }


        public void Filter()
        {
            _NameFilter = new StudentsNameFilter(_FilterNameParam);
            _ModuleFilter = new ActiveModuleFilter(_FilterModuleParam.Value);
            var filtered = _NameFilter.Filter(CoursesUnfiltered);
            filtered = _ModuleFilter.Filter(filtered);
            Courses = new ObservableCollection<Course>(filtered);
        }

        private void ChangeExpandState(bool state)
        {
            foreach(Course course in Courses)
            {
                course.Expanded = state;
                foreach(Group group in course.Groups)
                {
                    group.Expanded = state;
                    foreach(Module module in group.Modules)
                    {
                        module.Expanded = state;
                    }
                }

            }
        }

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

        private List<Course> CoursesUnfiltered { get; set; }

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
                SelectedModule = _SelectedGroup?.LastModule;
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
                    {
                        using (var _context = new ApplicationContext())
                        {
                            var module = _context.Modules.FirstOrDefault(x => x.ID == _SelectedModule.ID);
                            AppState.I.SelectedModule = module;
                            _SelectedModule = module;
                        }
                        
                    }
                        

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
                CoursesUnfiltered = _context.Courses
                                                          .Include(x => x.Groups
                                                          .Select(t => t.Modules
                                                          .Select(m => m.Students
                                                          .Select(q => q.Person)))).ToList();
            }
            EventsManager.ObjectChangedEvent += EventsManager_ObjectChangedEvent;
            AppState.I.PropertyChanged += StatePropertyChanged;

            CurrentView = view;
            CurrentView.DataContext = this;
            CurrentPageVM = new JournalPageVM(EditPersonPage, CertificateStudentPage);
            CurrentView.Show();
        }

        private void EventsManager_ObjectChangedEvent(object sender, ObjectChangedEventArgs e)
        {
            if (e.ObjectChanged is Group || e.ObjectChanged is Course || e.ObjectChanged is Student || e.ObjectChanged is Module
                || e.ObjectChanged is Person)
            {
                using (var _context = new ApplicationContext())
                {
                    Courses = new ObservableCollection<Course>(_context.Courses
                                                           .Include(x => x.Groups
                                                           .Select(t => t.Modules
                                                           .Select(m => m.Students
                                                           .Select(q => q.Person)))));
                    CoursesUnfiltered = _context.Courses
                                                           .Include(x => x.Groups
                                                           .Select(t => t.Modules
                                                           .Select(m => m.Students
                                                           .Select(q => q.Person)))).ToList();
                }
            }
        }

        private void StatePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "SelectedModule":
                    if (this.SelectedModule != AppState.I.SelectedModule)
                        this.SelectedModule = AppState.I.SelectedModule;
                    break;
                case "SelectedGroup":
                    if (this.SelectedGroup != AppState.I.SelectedGroup)
                        this.SelectedGroup = AppState.I.SelectedGroup;
                    break;
            }
        }

        #endregion
    }
}
