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
    /// Логика взаимодействия для UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl, IFrame
    {

        public int CountElements
        {
            get { return (int)GetValue(CountElementsProperty); }
            set { SetValue(CountElementsProperty, value); }
        }
        // Using a DependencyProperty as the backing store for CountElements.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CountElementsProperty =
            DependencyProperty.Register("CountElements", typeof(int), typeof(UserControl1), new PropertyMetadata(0));

        public UserControl1()
        {
            InitializeComponent();
        }
    }
}
