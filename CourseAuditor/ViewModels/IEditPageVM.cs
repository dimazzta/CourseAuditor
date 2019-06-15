using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CourseAuditor.ViewModels
{
    public interface IEditPageVM
    {
        ICommand GoBack { get; set; }
    }
}
