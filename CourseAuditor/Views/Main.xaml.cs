﻿using CourseAuditor.Helpers;
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
using System.Windows.Shapes;

namespace CourseAuditor.Views
{
    /// <summary>
    /// Логика взаимодействия для Main.xaml
    /// </summary>
    public partial class Main : Window, IView
    {
        public IPage CurrentFrame { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Main()
        {
            InitializeComponent();
        }

        private void TVCourseGroups_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue is Group)
                (DataContext as MainVM).SelectedGroup = e.NewValue as Group;
            if (e.NewValue is Module)
                (DataContext as MainVM).SelectedModule = e.NewValue as Module;
        }


        private void TVCourseGroups_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Right)
            {
                TreeViewItem treeViewItem = VisualUpwardSearch(e.OriginalSource as DependencyObject);
                if (treeViewItem != null)
                {
                    treeViewItem.Focus();
                    if (treeViewItem.DataContext is Course)
                    {
                        AppState.I.SelectedContextCourse = treeViewItem.DataContext as Course;
                        ContextMenu CourseMenu = TVCourseGroups.Resources["CourseMenu"] as ContextMenu;
                        CourseMenu.PlacementTarget = treeViewItem;
                        CourseMenu.IsOpen = true;
                    }
                    else if (treeViewItem.DataContext is Group)
                    {
                        AppState.I.SelectedContextGroup = treeViewItem.DataContext as Group;
                        ContextMenu GroupMenu = TVCourseGroups.Resources["GroupMenu"] as ContextMenu;
                        GroupMenu.PlacementTarget = treeViewItem;
                        GroupMenu.IsOpen = true;
                    }
                    else if (treeViewItem.DataContext is Module)
                    {
                        AppState.I.SelectedContextModule = treeViewItem.DataContext as Module;
                        ContextMenu ModuleMenu = TVCourseGroups.Resources["ModuleMenu"] as ContextMenu;
                        ModuleMenu.PlacementTarget = treeViewItem;
                        ModuleMenu.IsOpen = true;
                    }
                    else if (treeViewItem.DataContext is Student)
                    {
                        AppState.I.SelectedContextStudent = treeViewItem.DataContext as Student;
                        ContextMenu StudentMenu = TVCourseGroups.Resources["StudentMenu"] as ContextMenu;
                        StudentMenu.PlacementTarget = treeViewItem;
                        StudentMenu.IsOpen = true;
                    }
                }
            }
        }
        TreeViewItem VisualUpwardSearch(DependencyObject source)
        {
            while (source != null && !(source is TreeViewItem))
                source = VisualTreeHelper.GetParent(source);

            return source as TreeViewItem;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var phrase = SearchPhrase.Text;
            (DataContext as MainVM).FilterNameCommand.Execute(phrase);
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var state = cbModules.IsChecked.Value ? 0 : 1;
            (DataContext as MainVM).FilterModuleCommand.Execute(state);
        }
    }
}
