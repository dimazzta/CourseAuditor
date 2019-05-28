using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CourseAuditor.Models
{
    [Table("Person_Parent")]
    public class PersonParent : INotifyPropertyChanged
    {
        private int id;
        private int personId;
        private int parentId;

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
        [Column("Parent_ID")]
        public int ParentId
        {
            get { return parentId; }
            set
            {
                parentId = value;
                OnPropertyChanged("ParentId");
            }
        }
        public virtual Parent Parent { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
