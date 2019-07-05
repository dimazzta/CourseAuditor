using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace CourseAuditor.ViewModels
{
    public class ActiveModuleBottomPanelVM : BaseVM, IPageVM
    {
        public string PageTitle => "";
        public ActiveModuleBottomPanelVM(ICommand addNewClass, ICommand saveChanges, ICommand discardChanges, ICommand closeModule)
        {
            AddNewClassCommand = addNewClass;
            SaveChangesCommand = saveChanges;
            DiscardChangesCommand = discardChanges;
            CloseModule = closeModule;
        }

        public ICommand AddNewClassCommand { get; set; }
        public ICommand SaveChangesCommand { get; set; }
        public ICommand DiscardChangesCommand { get; set; }
        public ICommand CloseModule { get; set; }

    }
}