﻿using System;
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
    /// Логика взаимодействия для AddMedicalDocWindow.xaml
    /// </summary>
    public partial class AddMedicalDocWindow : Window, IView
    {
        public AddMedicalDocWindow()
        {
            InitializeComponent();
        }

        public IPage CurrentFrame { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
