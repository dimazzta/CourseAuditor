using System;
using Word = Microsoft.Office.Interop.Word;
using Spire.Doc;
using System.Drawing.Printing;
using System.Windows.Controls;
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

                // Закрываем документ
                doc.SaveAs2(Put);
                doc.Close();
                doc = null;
            
           
        }
        public void Print() // печать дока
        {
            string localPut = Put ;
            Document doc = new Document();
            doc.LoadFromFile(localPut);
            PrintDialog dialog = new PrintDialog();

            PrintDocument printDoc = doc.PrintDocument;
            printDoc.Print();
        }


    }
}


