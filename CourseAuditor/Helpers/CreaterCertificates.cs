using System;
using Word = Microsoft.Office.Interop.Word;
using Spire.Doc;
using System.Drawing.Printing;
using System.Windows.Controls;
using System.Windows.Documents;

namespace CourseAuditor.Helpers
{
    public class CreaterCertificates
    {
        string Put = Environment.CurrentDirectory + @"\Debug.doc";

        public CreaterCertificates(string name) //создание дока
        {
            string source = Environment.CurrentDirectory +  @"\DiplomUchastnika.doc";
            // Создаём объект документа
            Word.Document doc = null;
           
            // Создаём объект приложения
            Word.Application app = new Word.Application();
            // Путь до шаблона документа

            // Открываем
            doc = app.Documents.Open(source);
            doc.Activate();

            // Добавляем информацию
            // wBookmarks содержит все закладки
            Word.Bookmarks wBookmarks = doc.Bookmarks;
            Word.Range wRange;
            int i = 0;
            string[] data = new string[1] { $"{name}" };
            foreach (Word.Bookmark mark in wBookmarks)
            {
                wRange = mark.Range;
                wRange.Text = data[i];
                i++;
            }

            object copies = "1";
            object pages = "";
            object range = Word.WdPrintOutRange.wdPrintAllDocument;
            object items = Word.WdPrintOutItem.wdPrintDocumentContent;
            object pageType = Word.WdPrintOutPages.wdPrintAllPages;
            object oTrue = true;
            object oFalse = false;
            Object missing = System.Type.Missing;
            doc.PrintOut(ref oTrue, ref oFalse, ref range, ref missing, ref missing, ref missing,
    ref items, ref copies, ref pages, ref pageType, ref oFalse, ref oTrue,
    ref missing, ref oFalse, ref missing, ref missing, ref missing, ref missing);
            // Закрываем документ
            doc.SaveAs2(Put);
            doc.Close();
            doc = null;
            
           
        }

    }
}


