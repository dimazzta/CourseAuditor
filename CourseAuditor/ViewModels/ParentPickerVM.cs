using CourseAuditor.Helpers;
using CourseAuditor.Models;
using CourseAuditor.ViewModels.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CourseAuditor.ViewModels
{
    public class ParentPickerVM: BaseVM, IPageVM
    {
        
        public ParentPickerVM(List<Parent> parents = null) {
            if (parents == null)
                Parents = new ObservableCollection<Parent>();
            else
                Parents = new ObservableCollection<Parent>(parents);
        }

        private ObservableCollection<Parent> _Parents;
        public ObservableCollection<Parent> Parents
        {
            get {
                return _Parents;
            }
            set {
                _Parents = value;
                OnPropertyChanged("Parents");
            }
        }

        public Parent SelectedParent { get; set; }

        private ICommand _AddParentCommand;
        public ICommand AddParentCommand =>
            _AddParentCommand ??
            (_AddParentCommand = new RelayCommand(
                (obj) =>
                {
                    var pickParentVM = new AddParentVM();
                    bool? result = DialogService.I.ShowDialog(pickParentVM);
                    if (result.HasValue)
                    {
                        if (result.Value)
                        {
                            var parent = pickParentVM.Parent;
                            Parents.Add(parent);
                        }
                    }
                }
                ));


        private ICommand _DeleteParentCommand;
        public ICommand DeleteParentCommand =>
            _DeleteParentCommand ??
            (_DeleteParentCommand = new RelayCommand(
                (obj) => {
                    if (SelectedParent != null)
                        Parents.Remove(SelectedParent);
                },
                (obj) =>
                {
                    return SelectedParent != null;
                }
                ));

        public string PageTitle => "Выбор родителя";
    }
}
