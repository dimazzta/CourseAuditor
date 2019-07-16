using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CourseAuditor.ViewModels
{
    public interface IValidatable
    {
        bool Validate();
        string Error { get; set; }
    }
}
