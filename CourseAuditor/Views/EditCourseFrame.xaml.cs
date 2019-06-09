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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CourseAuditor.Views
{
    /// <summary>
    /// Логика взаимодействия для EditCourseFrame.xaml
    /// </summary>
    public partial class EditCourseFrame : UserControl , IFrame
    {
        public EditCourseFrame()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as EditCourseVM).AddCourse(tbCourseName.Text, tbCoursePrice.Text, tbCourseLessonsCount.Text);
        }
    }
}
