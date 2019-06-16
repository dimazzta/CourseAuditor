﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseAuditor.Models
{
    public class Group : ObservableObject, IExpandable
    {
        private bool _Expanded;
        [NotMapped]
        public bool Expanded
        {
            get
            {
                return _Expanded;
            }
            set
            {
                _Expanded = value;
                OnPropertyChanged("Expanded");
            }
        }

        [ForeignKey("Course")]
        public int Course_ID { get; set; }
        private Course _Course;
        private string _Title;
        private ICollection<Module> _Modules;

        [NotMapped]
        public Module LastModule
        {
            get
            {
                return Modules.OrderBy(x => x.DateStart).LastOrDefault();
            }
        }

        public virtual  ICollection<Module> Modules
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
                return  _Title;
            }
            set
            {
                _Title = value;
                OnPropertyChanged("Title");
            }
        }
        public override string ToString()
        {
            return $"ID - {ID} Название: {Title}";
        }



    }
}
