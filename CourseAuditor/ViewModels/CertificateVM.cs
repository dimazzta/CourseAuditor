using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourseAuditor.Views;

namespace CourseAuditor.ViewModels
{
    public class CertificateVM:BaseVM
    {
        public IView Certificate { get; set; }

        public CertificateVM(IView certificate)
        {
            Certificate = certificate;
        }



    
    }
}
