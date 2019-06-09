using CourseAuditor.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CourseAuditor.ViewModels
{
    public class AppState : INotifyPropertyChanged
    {
        // Синглтон. Хранит разделяемые данные. На нужные данные подписываемся из страниц / окон
        // Если станет слишком большим, надо будет разбить под каждую IViewVM
        private AppState() { }
        private static AppState _I;
        public static AppState I => _I ?? (_I = new AppState());

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private Group _SelectedGroup;
        public Group SelectedGroup
        {
            get
            {
                return _SelectedGroup;
            }
            set
            {
                _SelectedGroup = value;
                OnPropertyChanged("SelectedGroup");
            }
        }

        private Module _SelectedModule;
        public Module SelectedModule
        {
            get
            {
                return _SelectedModule;
            }
            set
            {
                _SelectedModule = value;
                OnPropertyChanged("SelectedModule");
            }
        }
    }
}
