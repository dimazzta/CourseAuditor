﻿using CourseAuditor.DAL;
using CourseAuditor.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Data.Entity;

namespace CourseAuditor.Models
{
    public class Module : ObservableObject
    {
        private Group _Group;
        private int _Number;
        private DateTime _DateStart;
        private DateTime? _DateEnd;
        private ICollection<Student> _Students;

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

        public override string ToString()
        {
            return "Модуль " + Number.ToString();
        }
    }
}