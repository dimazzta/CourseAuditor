using CourseAuditor.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class JournalFrame : UserControl, IFrame
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
            Students.Columns[0].IsReadOnly = true;
            Students.Columns[0].CanUserSort = true;
        }

        private void Students_PreparingCellForEdit(object sender, DataGridPreparingCellForEditEventArgs e)
        {
            (DataContext as JournalFrameVM).BeforeCellChangedHandler(e);
        }

        private void Students_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            (DataContext as JournalFrameVM).CellChangedHanlder(e);
        }
    }
}
