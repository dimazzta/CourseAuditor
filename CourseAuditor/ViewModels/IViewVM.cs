using CourseAuditor.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CourseAuditor.ViewModels
{
    // Интерфейс VM вью (окна). Пока одно, возможно будет отдельное окно на печать
    public interface IViewVM
    {
        IPageVM CurrentPageVM { get; set; }
        IView CurrentView { get; set; }
        ICommand ChangePage { get; }
    }
}
