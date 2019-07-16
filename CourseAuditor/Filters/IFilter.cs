using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseAuditor.Filters
{
    public interface IFilter<T>
    {
        ICollection<T> Filter(ICollection<T> sourse);
    }
}
