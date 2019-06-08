using CourseAuditor.DAL;
using CourseAuditor.Helpers;
using CourseAuditor.Models;
using CourseAuditor.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public JournalPageVM(IViewVM parent)
        {
            ParentViewVM = parent;

            //(ParentViewVM as MainVM).SelectedModule.PropertyChanged += (s, e) => this.SelectedModule = (ParentViewVM as MainVM).SelectedModule;
            //_context = (parent as MainVM)._context;
            using (var _context = new ApplicationContext())
                Assessments = new ObservableCollection<Assessment>(_context.Assessments);

        }

        #region Props
        public IViewVM ParentViewVM { get; private set; }

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
            table.Columns.Add("Студент", typeof(Student));

            List<Student> students;
            using (var _context = new ApplicationContext())
            {
                students = _context.Students
                    .Where(x => x.Group.ID == module.Group.ID && x.DateStart >= module.DateStart && x.DateStart < module.DateEnd)
                    .Include(x => x.Journals
                    .Select(t => t.Assessment))
                    .Include(x => x.Person)
                    .ToList();
            }
            List<DateTime> columns = students[0].Journals.Select(x => x.Date).ToList();
            foreach (var column in columns)
            {
                table.Columns.Add(column.ToString("dd-MM"), typeof(Journal));
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
                            var bJournal = _context.Journals.First(x => x.ID == journal.ID);
                            var bAssessment = _context.Assessments.First(x => x.ID == journal.Assessment.ID);
                            bJournal.Assessment = bAssessment;
                        }
                    }
                }
                _context.SaveChanges();
            }
            HasChanges = false;
            SaveChangesCommand.RaiseCanExecuteChanged();
            DiscardChangesCommand.RaiseCanExecuteChanged();
        }

        private void DiscardChanges()
        {
            HasChanges = false;
            UpdateJournal(_SelectedModule);
            SaveChangesCommand.RaiseCanExecuteChanged();
            DiscardChangesCommand.RaiseCanExecuteChanged();
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
            if (selectedColumn != 0)
            {
                ((e.Row.Item as DataRowView).Row[selectedColumn] as Journal).Assessment = SelectedAssessment;
                HasChanges = true;
                SaveChangesCommand.RaiseCanExecuteChanged();
                DiscardChangesCommand.RaiseCanExecuteChanged();
            }

        }

        public void BeforeCellChangedHandler(DataGridPreparingCellForEditEventArgs e)
        {
            int selectedColumn = e.Column.DisplayIndex;
            if (selectedColumn != 0)
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

        #endregion

        
    }
}
