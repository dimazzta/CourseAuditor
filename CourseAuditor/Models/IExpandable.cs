using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseAuditor.Models
{
    public interface IExpandable
    {
        bool Expanded { get; set; }
    }
}
