using CourseAuditor.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseAuditor.ViewModels
{
    // Интерфейс VM страницы. Страница - контент динамической области окна
    public interface IPageVM: INotifyPropertyChanged
    {
        string PageTitle { get; }
    }
}
