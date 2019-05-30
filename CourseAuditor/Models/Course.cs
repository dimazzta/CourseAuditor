using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseAuditor.Models
{
    public class Course : ObservableObject
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
    }
}
