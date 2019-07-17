using CourseAuditor.Helpers;
using CourseAuditor.Models;
using CourseAuditor.ViewModels;
using CourseAuditor.ViewModels.Dialogs;
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
        }

        private void Students_PreparingCellForEdit(object sender, DataGridPreparingCellForEditEventArgs e)
        {
            
            if ((e.Row.Item as DataRowView).Row[e.Column.DisplayIndex] is Journal 
                && (DataContext as JournalPageVM).SelectedModule.IsClosed == 0)
            {
                (DataContext as JournalPageVM).BeforeCellChangedHandler(e);
            }
            else if (e.Column.Header.ToString() == Constants.BalanceColumnName)
            {
                Students.CancelEdit();
                var student = (e.Row.Item as DataRowView).Row[Constants.StudentColumnName] as Student;

                var paymentVM = new PaymentVM(student);
                DialogService.I.ShowDialog(paymentVM);
                Students.CancelEdit();
            }
            else if (e.Column.Header.ToString() == Constants.StudentColumnName
                 && (DataContext as JournalPageVM).SelectedModule.IsClosed == 0)
            {
                Students.CancelEdit();
                var student = (e.Row.Item as DataRowView).Row[Constants.StudentColumnName] as Student;

                var docVM = new AddMedicalDocVM(student);
                DialogService.I.ShowDialog(docVM);
                
            }
            else
            {
                Students.CancelEdit();
            }
        }

        private void Students_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if ((DataContext as JournalPageVM).SelectedModule.IsClosed == 0)
                (DataContext as JournalPageVM).CellChangedHanlder(e);
        }

        private void Students_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Students.UnselectAllCells();
                DataGridCell dataGridCell = VisualUpwardSearch(e.OriginalSource as DependencyObject);
                Student student = (dataGridCell.DataContext as DataRowView)[dataGridCell.Column.DisplayIndex] as Student;
                if (student != null)
                {
                    AppState.I.SelectedContextStudent = student;
                    dataGridCell.IsSelected = true;
                    ContextMenu CourseMenu = Students.Resources["StudentMenu"] as ContextMenu;
                    CourseMenu.PlacementTarget = dataGridCell;
                    CourseMenu.IsOpen = true;
                }
            }
            catch
            {

            }
            
           
        }

        DataGridCell VisualUpwardSearch(DependencyObject source)
        {
            while (source != null && !(source is DataGridCell))
                source = VisualTreeHelper.GetParent(source);

            return source as DataGridCell;
        }
    }
}
