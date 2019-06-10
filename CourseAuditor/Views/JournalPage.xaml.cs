using CourseAuditor.Models;
using CourseAuditor.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CourseAuditor.Views
{
    /// <summary>
    /// Логика взаимодействия для JournalFrame.xaml
    /// </summary>
    public partial class JournalFrame : UserControl, IPage
    {
        public JournalFrame()
        {
            InitializeComponent();
            Students.PreparingCellForEdit += Students_PreparingCellForEdit;
            Students.CellEditEnding += Students_CellEditEnding;
            
            var dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(DataGrid));
            if (dpd != null)
            {
                dpd.AddValueChanged(Students, ItemsSourceChangedHanlder);
            }
        }


        private void ItemsSourceChangedHanlder(object sender, EventArgs e)
        {
            if (Students.Columns.Count > 0)
            {
                Students.Columns[0].IsReadOnly = true;
                Students.Columns[0].CanUserSort = true;
            }
        }

        private void Students_PreparingCellForEdit(object sender, DataGridPreparingCellForEditEventArgs e)
        {
            var block = (e.Row.Item as DataRowView).Row[e.Column.DisplayIndex] is Journal;
            if (block)
            {
                (DataContext as JournalPageVM).BeforeCellChangedHandler(e);
            }
            else
            {
                Students.CancelEdit();
            }
        }

        private void Students_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            (DataContext as JournalPageVM).CellChangedHanlder(e);
        }

      
    }
}
