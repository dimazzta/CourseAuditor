using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace CourseAuditor.Models
{
    public class Student : INotifyPropertyChanged
    {
        private int id;
        private int personId;
        private DateTime dateStart;
        private DateTime? dateEnd;
        private int groupId;

        public int Id
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }

     
        [Column("Person_ID")]
        public int PersonId
        {
            get { return personId; }
            set
            {
                personId = value;
                OnPropertyChanged("PersonId");
            }
        }
        public virtual Person Person { get; set; }

        [Column("Date_Start")]
        public DateTime DateStart
        {
            get { return dateStart; }
            set
            {
                dateStart = value;
                OnPropertyChanged("DateStart");
            }
        }

        [Column("Date_End")]
        public DateTime? DateEnd
        {
            get { return dateEnd; }
            set
            { 
                dateEnd = value;
                OnPropertyChanged("DateEnd");
            }
        }

        [Column("Group_ID")]
        public int GroupId
        {
            get { return groupId; }
            set
            {
                groupId = value;
                OnPropertyChanged("GroupId");
            }
        }

        public virtual Group Group { get; set; }
        
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
