﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseAuditor.Views
{
    public interface IFrame
    {
        // Something like GetDataContext ?
        object DataContext { get; set; }
    }
}
