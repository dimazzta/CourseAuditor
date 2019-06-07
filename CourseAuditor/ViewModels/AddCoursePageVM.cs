using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseAuditor.ViewModels
{
    public class AddCoursePageVM : BaseVM, IPageVM
    {
        public AddCoursePageVM(IViewVM parent)
        {
            ParentViewVM = parent;
        }
        public IViewVM ParentViewVM { get; private set; }
    }
}
