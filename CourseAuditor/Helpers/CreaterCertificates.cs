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
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace CourseAuditor.Helpers
{
    public class CreaterCertificates
    {
        public CreaterCertificates(string templatePath, string savePath, List<Student> students)
        {
            if (students.Count > 0)
            {
                Word.Application app = new Word.Application();
                var newDocument = app.Documents.Open(templatePath, ReadOnly: false);
                var r = new Random();
               
                string path = $"{savePath}\\Сертификаты_{DateTime.Now.ToString("dd-MMM-yyyy")}_{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}";
                newDocument.SaveAs2(path);

                foreach (var s in students)
                {
                    Student student;
                    //using (var _context = new ApplicationContext())
                    //{
                    //    student = _context.Students.Include(x => x.Person).Include(x => x.Module.Group.Course).First(x => x.ID == s.ID);
                    //}
                    Word.Document doc = app.Documents.Open(templatePath, ReadOnly: false);
                    doc.Activate();


                    var range = doc.Range();
                    range.Find.Execute(FindText: "#УЧЕНИК", Replace: WdReplace.wdReplaceAll, ReplaceWith: s.Person.FullName);
                    range.Find.Execute(FindText: "#КУРС", Replace: WdReplace.wdReplaceAll, ReplaceWith: s.Module.Group.Course.Name);
                    range.Find.Execute(FindText: "#ГРУППА", Replace: WdReplace.wdReplaceAll, ReplaceWith: s.Module.Group.Title);

                    var shapes = doc.Shapes;
                    foreach (Microsoft.Office.Interop.Word.Shape shape in shapes)
                    {
                        try
                        {
                            var initialText = shape.TextFrame.TextRange.Text;
                            var resultingText = initialText
                                .Replace("#УЧЕНИК", s.Person.FullName)
                                .Replace("#КУРС", s.Module.Group.Course.Name)
                                .Replace("#ГРУППА", s.Module.Group.Title);
                            if (initialText != resultingText)
                                shape.TextFrame.TextRange.Text = resultingText;
                        }
                        catch
                        {
                            continue;
                        }
                    }

                    doc.ActiveWindow.Selection.WholeStory();
                    doc.ActiveWindow.Selection.Copy();
                    newDocument.ActiveWindow.Selection.Paste();
                    object times = 1;
                    while (doc.Undo(ref times))
                    { }
                    doc.Close();
                    Marshal.ReleaseComObject(doc);
                }
                WordHelpers.PrintDocument(newDocument);
                newDocument.Paragraphs.Last.Range.Delete();
                newDocument.SaveAs2(path);
                app.Quit();
                Marshal.ReleaseComObject(app);
                Marshal.ReleaseComObject(newDocument);
                GC.Collect();
            }
            
        }
    }
}


