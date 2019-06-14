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
            // такс такс такс инъекция зависимостей подъехала
            DialogService.I.Register<DateTimeDialogVM, DateTimeDialogWindow>();
            DialogService.I.Register<AddMedicalDocVM, AddMedicalDocWindow>();
            DialogService.I.Register<PaymentVM, PaymentWindow>();
            DialogService.I.Register<AddParentVM, AddNewParentWindow>();

            new MainVM(new Main()); // это можно сделать аналогичным способом но пока оставим так

            //new AddParentVM(new AddNewParentWindow());
        }
    }
}
