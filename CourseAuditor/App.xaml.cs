using CourseAuditor.Helpers;
using CourseAuditor.ViewModels;
using CourseAuditor.ViewModels.Dialogs;
using CourseAuditor.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace CourseAuditor
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            DialogService.I.Register<DateTimeDialogVM, DateTimeDialogWindow>();
             new MainVM(new Main());
           //new CreaterCertificates().Creater("ЫЫЫ");

        }
    }
}
