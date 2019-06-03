using CourseAuditor.Models;
using CourseAuditor.ViewModels;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace CourseAuditor.Views
{
    /// <summary>
    /// Логика взаимодействия для Main.xaml
    /// </summary>
    public partial class Main : Window
    {
        public Main()
        {
            InitializeComponent();
            DataContext = new MainVM();
            //верная ветка
        }

        private void TVCourseGroups_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue is Group)
            {
                Group group = e.NewValue as Group;
                if (group != (DataContext as MainVM).SelectedGroup)
                {
                    //keep SelectedItem in sync with Treeview.SelectedItem
                    (DataContext as MainVM).SelectedGroup = e.NewValue as Group;
                    (DataContext as MainVM).Students = new System.Collections.ObjectModel.ObservableCollection<Student>((e.NewValue as Group).Students);
                }
            }
        }


    }
}
