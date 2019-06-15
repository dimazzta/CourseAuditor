using CourseAuditor.Helpers;
using CourseAuditor.ViewModels.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CourseAuditor.ViewModels
{
    public class DateTimeDialogVM : BaseVM, IDialogRequestClose
    {
        public DateTimeDialogVM(string message, DateTime currentDate)
        {
            Message = message;
            this.PickedDate = currentDate;
        }

        public event EventHandler<DialogCloseRequestedEventArgs> CloseRequested;
        public string Message { get; }

        private DateTime _PickedDate;
        public DateTime PickedDate
        {
            get
            {
                return _PickedDate;
            }
            set
            {
                _PickedDate = value;
                OnPropertyChanged("PickedDate");
            }
        }

        private ICommand _OkCommand;
        public ICommand OkCommand =>
            _OkCommand ??
            (_OkCommand = new RelayCommand(
                (obj) =>
                {
                    CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(true));
                }
            ));

        private ICommand _CancelCommand;
        public ICommand CancelCommand =>
            _CancelCommand ??
            (_CancelCommand = new RelayCommand(
                (obj) =>
                {
                    CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(false));
                }
            ));
    }
}
