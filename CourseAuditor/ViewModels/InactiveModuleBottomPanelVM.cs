using CourseAuditor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseAuditor.ViewModels
{
    public class InactiveModuleBottomPanelVM: BaseVM, IPageVM
    {
        public string PageTitle => "";
        public InactiveModuleBottomPanelVM(Module selectedModule)
        {
            SelectedModule = selectedModule;
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
