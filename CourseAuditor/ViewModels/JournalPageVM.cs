using CourseAuditor.DAL;
using CourseAuditor.Helpers;
using CourseAuditor.Models;
using CourseAuditor.ViewModels.Dialogs;
using CourseAuditor.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CourseAuditor.ViewModels
{
    public class JournalPageVM : BaseVM, IPageVM
    {
        public JournalPageVM(ICommand goToEditPage, ICommand goToCertificatesPage)
        {
            SelectedModule = AppState.I.SelectedModule;
            using (var _context = new ApplicationContext())
            {
                Assessments = new ObservableCollection<Assessment>(_context.Assessments);
            }
            // Подписка

            AppState.I.PropertyChanged += StatePropertyChanged;
            EventsManager.ObjectChangedEvent += (s, e) =>
            {
                if ((e.ObjectChanged is Payment || e.ObjectChanged is MedicalDoc
                || e.ObjectChanged is Student || e.ObjectChanged is Person || e.ObjectChanged is Return))
                {
                    UpdateJournal(SelectedModule);
                }
            };

            EditPersonPage = goToEditPage;
            CertificateStudentPage = goToCertificatesPage;

        }


        private void SelectProperBottomPanel()
        {
            if (SelectedModule != null)
            {
                if (SelectedModule.IsClosed == 0)
                    CurrentPanel = new ActiveModuleBottomPanelVM(AddNewClassCommand, SaveChangesCommand, DiscardChangesCommand, CloseModule);
                else
                    CurrentPanel = new InactiveModuleBottomPanelVM(SelectedModule);
            } 
        }

        public ICommand EditPersonPage { get; set; }
        public ICommand CertificateStudentPage { get; set; }

        private IPageVM _CurrentPanel;
        public IPageVM CurrentPanel
        {
            get
            {
                return _CurrentPanel;
            }
            set
            {
                _CurrentPanel = value;
                OnPropertyChanged("CurrentPanel");
            }
        }

        // Обработчик изменения состояния приложения
        private void StatePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // В данном случае нас интересует только одно свойство
            switch (e.PropertyName)
            {
                case "SelectedModule":
                    this.SelectedModule = AppState.I.SelectedModule;
                    break;
            }
        }


        #region Props

        private bool _HasChanges;
        public bool HasChanges
        {
            get
            {
                return _HasChanges;
            }
            set
            {
                _HasChanges = value;
                OnPropertyChanged("HasChanges");
            }
        }

        public ObservableCollection<Assessment> Assessments { get; set; }
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

        private Assessment _SelectedAssessment;
        public Assessment SelectedAssessment
        {
            get
            {
                return _SelectedAssessment;
            }
            set
            {
                _SelectedAssessment = value;
                OnPropertyChanged("SelectedAssessment");
            }
        }

        internal void BeforeCellChangedHandler1(object item)
        {
            SelectedAssessment = (item as Journal).Assessment;
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
                    UnsavedChangesPrompt();
                    _SelectedModule = value;
                    Students = value == null ? new ObservableCollection<Student>() : new ObservableCollection<Student>(value.Students);
                    UpdateJournal(_SelectedModule);
                    RefreshPageTitle();
                    SelectProperBottomPanel();
                    OnPropertyChanged("SelectedModule");
                }
            }
        }

        private int _IsClosed;
        public int IsClosed
        {
            get
            {
                return _IsClosed;
            }
            set
            {
                _IsClosed = value;
                OnPropertyChanged("IsClosed");
            }
        }

        private DataTable _Table;
        public DataTable Table
        {
            get
            {
                return _Table;
            }
            set
            {
                _Table = value;
                OnPropertyChanged("Table");
            }
        }

        public List<Tuple<Journal, Assessment>> TableInitialValues;
        #endregion

        #region Methods
        private void UpdateJournal(Module module)
        {
            Table = new DataTable();

            if (module != null)
            {
                using (var _context = new ApplicationContext())
                {
                    module = _context.Modules.Include(x => x.Students.Select(t => t.Person)).FirstOrDefault(x => x.ID == module.ID);
                }
                if (module.Students.Count > 0)
                {
                    DataTable table = new DataTable();
                    table.Columns.Add(Constants.StudentColumnName, typeof(Student));
                    table.Columns.Add(Constants.BalanceColumnName, typeof(double));
                    List<Student> students;
                    using (var _context = new ApplicationContext())
                    {
                        students = _context.Students
                        .Where(x => x.Module.ID == module.ID)
                        .Include(x => x.Journals
                        .Select(t => t.Assessment))
                        .Include(x => x.Person)
                        .Include(x => x.Module)
                        .ToList();
                    }

                    List<DateTime> columns = students
                                        .First(x => x.Journals.Count == students
                                        .Max(t => t.Journals.Count)).Journals
                                        .Select(x => x.Date)
                                        .OrderBy(x => x.Date)
                                        .ToList();

                    foreach (var column in columns)
                    {
                        try
                        {
                            table.Columns.Add(column.ToString("dd MMM"), typeof(Journal));
                        }
                        catch
                        {
                            continue;
                        }
                    }


                    foreach (var student in students)
                    {
                        student.RecalculateBalance();
                        List<Journal> journals = student.Journals.OrderBy(x => x.Date).ToList();

                        DataRow row = table.NewRow();
                        row[Constants.StudentColumnName] = student;
                        foreach (var j in journals)
                            row[j.Date.ToString("dd MMM")] = j;

                        row[Constants.BalanceColumnName] = student.Balance;
                        table.Rows.Add(row);
                    }

                    Table = table;
                    
                }
            }
        }

        private void AddNewClass()
        {
            var currentDate = DateTime.Now;
            var dialog = new DateTimeDialogVM("Пожалуйста, выберите дату", currentDate);
            bool? result = DialogService.I.ShowDialog(dialog);

            if (result.HasValue)
            {
                if (result.Value)
                {
                    currentDate = dialog.PickedDate;
                    var columnName = currentDate.ToString("dd MMM");
                    try
                    {
                        Table.Columns.Add(columnName, typeof(Journal));
                    }
                    catch
                    {
                        MessageBox.Show("Данная дата уже есть в таблице."); // TODO: нумерация одинаковых дат в таблице
                        return;
                    }

                    foreach (DataRow row in Table.Rows)
                    {
                        var journal = new Journal()
                        {
                            Date = currentDate,
                            Student = row[0] as Student,
                            Assessment = Assessments.First(x => x.Title == ""),
                            ID = 0
                        };
                        row[columnName] = journal;
                    }
                    HasChanges = true;
                    Table = Table.Copy(); // чтобы обновить UI. Не самое эффективное решение, но зато VM по прежнему ничего не знает о V
                }

            }
        }

        private void SaveChanges()
        {
            using (var _context = new ApplicationContext())
            {
                int rows = Table.Rows.Count;
                int cols = Table.Columns.Count;
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        if (Table.Rows[i][j] is Journal)
                        {
                            var journal = Table.Rows[i][j] as Journal;
                            var bJournal = _context.Journals.FirstOrDefault(x => x.ID == journal.ID);
                            var bAssessment = _context.Assessments.First(x => x.ID == journal.Assessment.ID);
                            if (bJournal != null)
                            {
                                bJournal.Assessment = bAssessment;
                                _context.Entry(bJournal).State = EntityState.Modified;
                            }
                            else
                            {
                                if (!_context.Journals.Any(x => x.Date == journal.Date))
                                {
                                    bJournal = new Journal()
                                    {
                                        Student = _context.Students.First(x => x.ID == journal.Student.ID),
                                        Date = journal.Date,
                                        Assessment = _context.Assessments.FirstOrDefault(x => x.ID == journal.Assessment.ID)
                                    };
                                    _context.Journals.Add(bJournal);
                                    _context.Entry(bJournal).State = EntityState.Added;
                                }
                            }

                        }
                    }
                }
                _context.SaveChanges();
                UpdateJournal(SelectedModule);
            }
            HasChanges = false;
        }

        private void DiscardChanges()
        {
            HasChanges = false;
            UpdateJournal(_SelectedModule);
        }

        private void UnsavedChangesPrompt()
        {
            if (HasChanges)
            {
                var result = MessageBox.Show("В журнале есть несохраненные данные. Сохранить перед переходом к новой группе?", "Несохраненные данные", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    SaveChanges();
                }
                else
                {
                    DiscardChanges();
                }
            }
        }

        #endregion

        #region Handlers
        public void CellChangedHanlder(DataGridCellEditEndingEventArgs e)
        {
            int selectedColumn = e.Column.DisplayIndex;
            if (selectedColumn != 0 && (e.Row.Item as DataRowView).Row[selectedColumn] is Journal)
            {
                ((e.Row.Item as DataRowView).Row[selectedColumn] as Journal).Assessment = SelectedAssessment;
                HasChanges = true;
            }

        }

        public void BeforeCellChangedHandler(DataGridPreparingCellForEditEventArgs e, Action Unfocus = null)
        {
            int selectedColumn = e.Column.DisplayIndex;
            if (selectedColumn != 0 && (e.Row.Item as DataRowView).Row[selectedColumn] is Journal)
            {
                SelectedAssessment = ((e.Row.Item as DataRowView).Row[selectedColumn] as Journal).Assessment;
            }
        
        }
        #endregion

        #region Commands
        private ICommand _SaveChangesCommand;
        public ICommand SaveChangesCommand =>
            _SaveChangesCommand ??
            (_SaveChangesCommand = new RelayCommand(
                (obj) =>
                {
                    SaveChanges();
                },
                (obj) =>
                {
                    return HasChanges;
                }
        ));

        private ICommand _DiscardChangesCommand;
        public ICommand DiscardChangesCommand =>
            _DiscardChangesCommand ??
            (_DiscardChangesCommand = new RelayCommand(
                (obj) =>
                {
                    DiscardChanges();
                },
                (obj) =>
                {
                    return HasChanges;
                }
        ));

        private ICommand _AddNewClassCommand;
        public ICommand AddNewClassCommand =>
            _AddNewClassCommand ??
            (_AddNewClassCommand = new RelayCommand(
                (obj) =>
                {
                    AddNewClass();
                },
                (obj) =>
                {
                    return _SelectedModule != null;
                }
                ));

        private ICommand _CloseModule;
        public ICommand CloseModule =>
            _CloseModule ?? (
            _CloseModule = new RelayCommand(
                (_) =>
                {
                    var result = MessageBox.Show("Вы уверены, что хотите закрыть модуль? " +
                        "После закрытия модуль будет невозможно открыть, будет невозможно редактировать отметки посещаемости и " +
                        "вносить справки, однако по прежнему можно будет оформлять возвраты и довносить платежи.", "Вы уверены?",
                        MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        SelectedModule.Close();
                        SelectProperBottomPanel();
                        UpdateJournal(SelectedModule);
                    }
                }
                ));
        #endregion

        private void RefreshPageTitle()
        {
            try
            {
                PageTitle = $"Журнал. Курс {SelectedModule.Group.Course.Name}, группа {SelectedModule.Group.Title}, модуль {SelectedModule.Number}";
            }
            catch
            {
                PageTitle = $"Главная";
            }
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
    }
}
