using CourseAuditor.DAL;
using CourseAuditor.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Data.Entity;

namespace CourseAuditor.Models
{
    public class Module : ObservableObject, IExpandable
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

        [ForeignKey("Group")]
        public int Group_ID { get; set; }

        private Group _Group;
        private int _Number;
        private DateTime _DateStart;
        private DateTime? _DateEnd;
        private ICollection<Student> _Students;
        private double _LessonPrice;
        private int _LessonCount;
        private int _IsClosed;

        public int IsClosed
        {
            get
            {
                return _IsClosed;
            }
            set
            {
                _IsClosed = value;
                OnPropertyChanged("IsClosed");
            }
        }

        public void Close()
        {
            using(var _context = new ApplicationContext())
            {
                var module = _context.Modules.FirstOrDefault(x => x.ID == ID);
                module.IsClosed = IsClosed = 1;
                module.DateEnd = DateEnd = DateTime.Now;
                _context.SaveChanges();
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



        public virtual Group Group
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
        public DateTime? DateEnd
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

        public override string ToString()
        {
            return "Модуль " + Number.ToString();
        }
    }
}