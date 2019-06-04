﻿using CourseAuditor.Helpers;
using CourseAuditor.Models;
using CourseAuditor.ViewModels;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace CourseAuditor.Views
{
    /// <summary>
    /// Логика взаимодействия для Main.xaml
    /// </summary>
    public partial class Main : Window, IView
    {
        public IFrame CurrentFrame { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Main()
        {
            InitializeComponent();
            Students.PreparingCellForEdit += Students_PreparingCellForEdit;
            Students.CellEditEnding += Students_CellEditEnding;
        }

        private void Students_PreparingCellForEdit(object sender, DataGridPreparingCellForEditEventArgs e)
        {
            Students.Columns[0].IsReadOnly = true; // хз что здесь можно придумать
            (DataContext as MainVM).BeforeCellChangedHandler(e);
        }

        private void Students_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            (DataContext as MainVM).CellChangedHanlder(e);
        }

        private void TVCourseGroups_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue is Group)
                  (DataContext as MainVM).SelectedGroup = e.NewValue as Group;
        }

    }
}
