using System;
using Word = Microsoft.Office.Interop.Word;
using Spire.Doc;
using System.Drawing.Printing;
using System.Windows.Controls;
using System.Windows.Documents;
using CourseAuditor.Models;
using CourseAuditor.DAL;
using System.Linq;
using System.Data.Entity;
using Microsoft.Office.Interop.Word;

namespace CourseAuditor.Helpers
{
    public class CreaterCertificates
    {
        public CreaterCertificates(string templatePath, string savePath, Student student)
        {
            using(var _context = new ApplicationContext())
            {
                student = _context.Students.Include(x => x.Person).Include(x => x.Module.Group.Course).First(x => x.ID == student.ID);
            }

            Word.Application app = new Word.Application();
            Word.Document doc = app.Documents.Open(templatePath, ReadOnly:false);
            doc.Activate();

            var range = doc.Range();
            range.Find.Execute(FindText: "#УЧЕНИК", Replace: WdReplace.wdReplaceAll, ReplaceWith: student.Person.FullName);
            range.Find.Execute(FindText: "#КУРС", Replace: WdReplace.wdReplaceAll, ReplaceWith: student.Module.Group.Course.Name);
            range.Find.Execute(FindText: "#ГРУППА", Replace: WdReplace.wdReplaceAll, ReplaceWith: student.Module.Group.Title);

            var shapes = doc.Shapes;
            foreach (Microsoft.Office.Interop.Word.Shape shape in shapes)
            {
                try
                {
                    var initialText = shape.TextFrame.TextRange.Text;
                    var resultingText = initialText
                        .Replace("#УЧЕНИК", student.Person.FullName)
                        .Replace("#КУРС", student.Module.Group.Course.Name)
                        .Replace("#ГРУППА", student.Module.Group.Title);
                    if (initialText != resultingText)
                        shape.TextFrame.TextRange.Text = resultingText;
                }
                catch
                {
                    continue;
                }
            }
            WordHelpers.PrintDocument(doc);
            doc.SaveAs2($"{savePath}\\{student.Person.FullName}_{DateTime.Now.ToString("dd-MMM-yyyy")}");
            doc.Close();
            doc = null;
            GC.Collect();
        }
    }
}


