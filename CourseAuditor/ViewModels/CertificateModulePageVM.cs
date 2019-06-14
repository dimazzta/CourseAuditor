using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CourseAuditor.DAL;
using CourseAuditor.Helpers;
using CourseAuditor.Models;
using CourseAuditor.Views;

namespace CourseAuditor.ViewModels
{
    public class CertificateModulePageVM : BaseVM, IPageVM
    {

        public CertificateModulePageVM(Module module)
        {
            using (ApplicationContext _context = new ApplicationContext())
            {
              var a =  _context.Modules
                    .Include(x => x.Students)
                    .Select(x => x.Students);
                
            }            
        }

        





    }
}
