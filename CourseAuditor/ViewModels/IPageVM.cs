using CourseAuditor.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseAuditor.ViewModels
{
    public interface IPageVM
    {
        IViewVM ParentViewVM { get; }
    }
}
