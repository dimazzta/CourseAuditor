using CourseAuditor.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CourseAuditor.Filters
{
    public class ActiveModuleFilter : IFilter<Course>
    {
        private int _Closed;
        public ActiveModuleFilter(int closed)
        {
            _Closed = closed;
        }

        public ICollection<Course> Filter(ICollection<Course> sourse)
        {
            if (_Closed == 1) return sourse;
            List<Course> src = new List<Course>();
            foreach (var course in sourse)
            {
                var newCourse = course.Clone() as Course;
                var groups = newCourse.Groups;

                newCourse.Groups = new List<Group>();
                foreach (var group in groups)
                {
                    var newGroup = group.Clone() as Group;
                    var modules = newGroup.Modules;
                    newGroup.Modules = new List<Module>();

                    foreach (var module in modules)
                    {
                        if (module.IsClosed == _Closed)
                        {
                            newGroup.Modules.Add(module);
                            newGroup.Expanded = true;
                        }
                    }
                    if (newGroup.Modules.Count > 0)
                    {
                        newCourse.Groups.Add(newGroup);
                        newCourse.Expanded = true;
                        src.Add(newCourse);
                    }
                }

            }

            return src;
        }
    }
}