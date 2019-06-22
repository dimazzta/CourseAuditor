using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseAuditor.Helpers
{
    public class WordHelpers
    {
        public static void SearchReplace()
        {
            
        }

 
  



    public static void PrintDocument(Document doc)
        {
            object copies = "1";
            object pages = "";
            object range = Microsoft.Office.Interop.Word.WdPrintOutRange.wdPrintAllDocument;
            object items = Microsoft.Office.Interop.Word.WdPrintOutItem.wdPrintDocumentContent;
            object pageType = Microsoft.Office.Interop.Word.WdPrintOutPages.wdPrintAllPages;
            object oTrue = true;
            object oFalse = false;
            object missing = Type.Missing;
            doc.PrintOut(ref oTrue, ref oFalse, ref range, ref missing, ref missing, ref missing,
                         ref items, ref copies, ref pages, ref pageType, ref oFalse, ref oTrue,
                         ref missing, ref oFalse, ref missing, ref missing, ref missing, ref missing);
        }
    }
}
