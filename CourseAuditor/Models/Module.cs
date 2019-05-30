using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseAuditor.Models
{
    public class Module : ObservableObject
    {
        public Group _Group;
        public int _Number;
        public DateTime _DateStart;
        public DateTime _DateEnd;

        public Group Group
        {
            get
            {
                return _Group;
            }
            set
            {
                _Group = value;
                OnPropertyChanged("Group");
            }
        }

        public int Number
        {
            get
            {
                return _Number;
            }
            set
            {
                _Number = value;
                OnPropertyChanged("Number");
            }
        }
        public DateTime DateStart
        {
            get
            {
                return _DateStart;
            }
            set
            {
                _DateStart = value;
                OnPropertyChanged("DateStart");
            }
        }
        public DateTime DateEnd
        {
            get
            {
                return _DateEnd;
            }
            set
            {
                _DateEnd = value;
                OnPropertyChanged("DateEnd");
            }
        }
        
    }
}