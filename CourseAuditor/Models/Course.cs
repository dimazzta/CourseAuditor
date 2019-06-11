﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseAuditor.Models
{
    public class Course : ObservableObject, INotifyPropertyChanged
    {
        private string _Name;
        private double _Price;
        private int _LessonsCount;
        private ICollection<Group> _Groups;

        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
                OnPropertyChanged("Name");
            }
        }

       
        public double Price
        {
            get
            {
                return _Price;
            }
            set
            {
                _Price = value;
                OnPropertyChanged("Price");
            }
        }

        
        public int LessonsCount
        {
            get
            {
                return _LessonsCount;
            }
            set
            {
                _LessonsCount = value;
                OnPropertyChanged("LessonsCount");
            }
        }

        [ForeignKey("Course_ID")]
        public virtual ICollection<Group> Groups
        {
            get {
                return _Groups;
            }
            set
            {
                _Groups = value;
                OnPropertyChanged("Groups");
            }
        }

        public override string ToString()
        {
            return $"ID - {ID} Название: {Name}";
        }
    }
}
