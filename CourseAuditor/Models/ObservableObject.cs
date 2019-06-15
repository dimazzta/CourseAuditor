using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CourseAuditor.Models
{
    public abstract class ObservableObject : INotifyPropertyChanged
    {
        private int _Id;
        public virtual int ID
        {
            get
            {
                return _Id;
            }
            set
            {
                _Id = value;
                OnPropertyChanged("ID");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName]string propertyName="")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
