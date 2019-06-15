using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseAuditor.Models
{
    public class Assessment : ObservableObject
    {
        public Assessment(string title)
        {
            Title = title;
        }
        public Assessment()
        {
        }
        private string _Title;
        public string Title
        {
            get
            {
                return _Title;
            }
            set
            {
                _Title = value;
                OnPropertyChanged("Title");
            }
        }
        private int _Type;
        public int Type
        {
            get
            {
                return _Type;
            }
            set
            {
                _Type = value;
                OnPropertyChanged("Type");
            }
        }
        public override string ToString()
        {
            return Title;
        }
    }
}
