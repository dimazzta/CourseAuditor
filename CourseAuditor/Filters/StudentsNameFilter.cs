using CourseAuditor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseAuditor.Filters
{
    public class StudentsNameFilter : IFilter<Course>
    {
        private string _Criteria;
        public StudentsNameFilter(string criteria)
        {
            _Criteria = criteria;
        }


        public ICollection<Course> Filter(ICollection<Course> source)
        {
            List<Course> src = new List<Course>();
            foreach(var course in source)
            {
                var newCourse = course.Clone() as Course;
                var groups = newCourse.Groups;

                newCourse.Groups = new List<Group>();
                foreach(var group in groups)
                {
                    var newGroup = group.Clone() as Group;
                    var modules = newGroup.Modules;
                    newGroup.Modules = new List<Module>();

                    foreach(var module in modules)
                    {
                        var newModule = module.Clone() as Module;
                        var students = newModule.Students;
                        newModule.Students = new List<Student>();

                        foreach(var student in students)
                        {
                            if (student.Person.FullName.IndexOf(_Criteria, StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                newModule.Students.Add(student);
                                newModule.Expanded = true;
                            }
                        }
                        if (newModule.Students.Count > 0)
                        {
                            newGroup.Modules.Add(newModule);
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
