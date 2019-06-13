using CourseAuditor.Helpers;
using CourseAuditor.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CourseAuditor.ViewModels
{
    public class ParentPickerVM: IPageVM
    {
        public ParentPickerVM() {
            Parents = new ObservableCollection<Parent>();
        }

        public ObservableCollection<Parent> Parents { get; set; }
        public Parent SelectedParent { get; set; }

        private ICommand _AddParentCommand;
        public ICommand AddParentCommand =>
            _AddParentCommand ??
            (_AddParentCommand = new RelayCommand(
                (obj) => {
                    // TODO: вызов формы добавления
                }
                ));

        private ICommand _DeleteParentCommand;
        public ICommand DeleteParentCommand =>
            _DeleteParentCommand ??
            (_DeleteParentCommand = new RelayCommand(
                (obj) => {
                    Parents.Remove(SelectedParent);
                },
                (obj) =>
                {
                    return SelectedParent != null;
                }
                ));
    }
}
