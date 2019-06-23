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

namespace CourseAuditor.ViewModels
{
    public class JournalPageVM : BaseVM, IPageVM
    {
        public JournalPageVM()
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
                    Students = value == null ?  new ObservableCollection<Student>() : new ObservableCollection<Student>(value.Students);
                    UpdateJournal(_SelectedModule);
                    RefreshPageTitle();
                    OnPropertyChanged("SelectedModule");
                }
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

        public void BeforeCellChangedHandler(DataGridPreparingCellForEditEventArgs e)
        {
            int selectedColumn = e.Column.DisplayIndex;
            if (selectedColumn != 0 && (e.Row.Item as DataRowView).Row[selectedColumn] is Journal)
            {
                SelectedAssessment = ((e.Row.Item as DataRowView).Row[selectedColumn] as Journal).Assessment;
            }

        }
        #endregion

        #region Commands
        private RelayCommand _SaveChangesCommand;
        public RelayCommand SaveChangesCommand =>
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

        private RelayCommand _DiscardChangesCommand;
        public RelayCommand DiscardChangesCommand =>
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

        private RelayCommand _AddNewClassCommand;
        public RelayCommand AddNewClassCommand =>
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

        #endregion


        private void RefreshPageTitle() => PageTitle = $"Журнал. Курс {SelectedModule.Group.Course.Name}, группа {SelectedModule.Group.Title}, модуль {SelectedModule.Number}";

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
