﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseAuditor.Models
{
    public class Group : ObservableObject, INotifyPropertyChanged
    {

        private Course _Course;
        private string _Title;
        private ICollection<Student> _Students;
        private ICollection<Module> _Modules;

        public virtual ICollection<Module> Modules
        {
            get
            {
                return _Modules;
            }
            set
            {
                _Modules = value;
                OnPropertyChanged("Modules");
            }
        }

        public virtual ICollection<Student> Students
        {
            get
            {
                return _Students;
            }
            set
            {
                _Students = value;
                OnPropertyChanged("Students");
            }
        }

        public virtual Course Course
        {
            get
            {
                return _Course;
            }
            set
            {
                _Course = value;
                OnPropertyChanged("Course");
            }
        }

       
        public string Title
        {
            get
            {
                return "Группа " + _Title;
            }
            set
            {
                _Title = value;
                OnPropertyChanged("Title");
            }
        }

        
    }
}
