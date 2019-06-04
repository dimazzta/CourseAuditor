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

namespace CourseAuditor.ViewModels
{
    public class MainVM : BaseVM
    {
        private ApplicationContext _context;


        public IView CurrentView { get; set; }
        public ObservableCollection<Course> Courses { get; set; }
        public ObservableCollection<Student> Students { get; set; }
        public ObservableCollection<Assessment> Assessments { get; set; }

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
                Students = new ObservableCollection<Student>(value.Students);
                UpdateJournal();
                OnPropertyChanged("SelectedGroup");
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

        private void UpdateJournal()
        {
            DataTable table = new DataTable();

            Module lastModule = _SelectedGroup.Modules.OrderBy(x => x.DateStart).Last();
            List<Student> students = _SelectedGroup.Students.ToList();
            table.Columns.Add("Студент");

            List<DateTime> columns = students[0].Journals.Where(x => x.Date.InRange(lastModule.DateStart, lastModule.DateEnd)).Select(x => x.Date).ToList();
            foreach (var column in columns)
            {
                var c = table.Columns.Add(column.ToString("dd-MM"), typeof(Assessment));
            }
      
            foreach (var student in students)
            {
                List<Journal> journals = student.Journals.Where(x => x.Date.InRange(lastModule.DateStart, lastModule.DateEnd)).ToList();
                DataRow row = table.NewRow();
                row[0] = student.Person.FullName;
                
                int i = 1;
                foreach (var j in journals)
                {
                    row[i] = j.Assessment;
                    i++;
                }
                table.Rows.Add(row);
            }
            Table = table;

        }
        
        public void CellChangedHanlder(DataGridCellEditEndingEventArgs e)
        {
            int selectedColumn = e.Column.DisplayIndex;
            if (selectedColumn != 0)
                (e.Row.Item as DataRowView).Row[selectedColumn] = SelectedAssessment;
        }

        public void BeforeCellChangedHandler(DataGridPreparingCellForEditEventArgs e)
        {
            int selectedColumn = e.Column.DisplayIndex;
            if (selectedColumn != 0)
                SelectedAssessment = (e.Row.Item as DataRowView).Row[selectedColumn] as Assessment;
        }

        public MainVM(IView view)
        {
            _context = new ApplicationContext();
            Courses = new ObservableCollection<Course>(_context.Courses);
            Assessments = new ObservableCollection<Assessment>(_context.Assessments);

            CurrentView = view;
            CurrentView.DataContext = this;
            CurrentView.Show();
        }
    }
}
