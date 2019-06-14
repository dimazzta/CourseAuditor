using Spire.Doc;
using System.Drawing.Printing;
using System.Windows.Controls;
namespace CourseAuditor.Helpers
{
    public class CertificateWriter
    {

        public void Writer(string s)
        {


            Document doc = new Document();
            doc.LoadFromFile($"{s}");
            PrintDialog dialog = new PrintDialog();

            PrintDocument printDoc = doc.PrintDocument;
            printDoc.Print();

            

        }

    }
}
