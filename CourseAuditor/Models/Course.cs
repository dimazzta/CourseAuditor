using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseAuditor.Models
{
    public class Course : ObservableObject, IExpandable, ICloneable
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

        private string _Name;
        private double _LessonPrice;
        private int _LessonCount;
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

       
        public double LessonPrice
        {
            get
            {
                return _LessonPrice;
            }
            set
            {
                _LessonPrice = value;
                OnPropertyChanged("LessonPrice");
            }
        }

        
        public int LessonCount
        {
            get
            {
                return _LessonCount;
            }
            set
            {
                _LessonCount = value;
                OnPropertyChanged("LessonCount");
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

        public object Clone()
        {
            return new Course() { Name = Name, Expanded = Expanded, Groups = Groups, ID = ID, LessonCount = LessonCount, LessonPrice = LessonPrice };
        }
    }
}
