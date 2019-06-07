using CourseAuditor.DAL;
using CourseAuditor.Helpers;
using CourseAuditor.Models;
using CourseAuditor.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CourseAuditor.ViewModels
{
    public class JournalPageVM : BaseVM, IPageVM
    {
        public JournalPageVM(IViewVM parent)
        {
            ParentViewVM = parent;
            _context = new ApplicationContext();
            
            Assessments = new ObservableCollection<Assessment>(_context.Assessments);
        }

        #region Props
        private ApplicationContext _context;
        public IViewVM ParentViewVM { get; private set; }

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
                    Students = new ObservableCollection<Student>(value.Students);
                    UpdateJournal(_SelectedModule);
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

            DataTable table = new DataTable();

            List<Student> students = module.Students.OrderBy(x => x.Person.FullName).ToList();
            table.Columns.Add("Студент", typeof(Student));

            List<DateTime> columns = students[0].Journals.Where(x => x.Date.InRange(module.DateStart, module.DateEnd)).Select(x => x.Date).OrderBy(x => x.Date).ToList();
            foreach (var column in columns)
            {
                var c = table.Columns.Add(column.ToString("dd-MM"), typeof(Journal));
            }

            foreach (var student in students)
            {
                List<Journal> journals = student.Journals.Where(x => x.Date.InRange(module.DateStart, module.DateEnd)).OrderBy(x => x.Date).ToList();
                
                DataRow row = table.NewRow();
                row[0] = student;
                int i = 1;
                foreach (var j in journals)
                    row[i++] = j;

                table.Rows.Add(row);
            }
            Table = table;
            TableInitialValues = new List<Tuple<Journal, Assessment>>();
            CopyValues(Table, TableInitialValues);  // Данная копия хранит изначальный слепок таблицы. На нее будем откатываться.
        }

        private void SaveChanges()
        {
            _context.SaveChanges();
            CopyValues(Table, TableInitialValues);  // Новый слепок только что сохраненной таблицы.
            SaveChangesCommand.RaiseCanExecuteChanged();
            DiscardChangesCommand.RaiseCanExecuteChanged();
        }

        private void DiscardChanges()
        {
            RestoreValues(TableInitialValues);  // Откат
            UpdateJournal(_SelectedModule);
            SaveChangesCommand.RaiseCanExecuteChanged();
            DiscardChangesCommand.RaiseCanExecuteChanged();
        }

        private bool HasChanges(List<Tuple<Journal, Assessment>> source)
        {
            bool result = true;
            if (source != null)
                foreach (var pair in source)
                {
                    result &= (pair.Item1 as Journal).Assessment == pair.Item2;
                }
            return !result;
        }

        private void CopyValues(DataTable source, List<Tuple<Journal, Assessment>> dest)
        {
            dest.Clear();
            int rows = source.Rows.Count;
            int cols = source.Columns.Count;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (source.Rows[i][j] is Journal)
                    {
                        dest.Add(new Tuple<Journal, Assessment>(source.Rows[i][j] as Journal, (source.Rows[i][j] as Journal).Assessment));
                    }
                }
            }
        }

        private void RestoreValues(List<Tuple<Journal, Assessment>> source)
        {
            foreach (var pair in source)
            {
                (pair.Item1 as Journal).Assessment = pair.Item2;
            }
        }

        private void UnsavedChangesPrompt()
        {
            if (HasChanges(TableInitialValues))
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
            if (selectedColumn != 0)
            {
                ((e.Row.Item as DataRowView).Row[selectedColumn] as Journal).Assessment = SelectedAssessment;
                SaveChangesCommand.RaiseCanExecuteChanged();
                DiscardChangesCommand.RaiseCanExecuteChanged();
            }

        }

        public void BeforeCellChangedHandler(DataGridPreparingCellForEditEventArgs e)
        {
            int selectedColumn = e.Column.DisplayIndex;
            if (selectedColumn != 0)
                SelectedAssessment = ((e.Row.Item as DataRowView).Row[selectedColumn] as Journal).Assessment;
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
                    return HasChanges(TableInitialValues);
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
                    return HasChanges(TableInitialValues);
                }
        ));

        #endregion

        
    }
}
