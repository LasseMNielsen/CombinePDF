using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using combinePDF.Objects;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace combinePDF.Functions
{
    class PDF
    {
        public static void combinePDF(string outputFile, List<listboxPdfObjects> files)
        {
            if (files.Count > 0)
            {
                using (var fileStream = new FileStream(outputFile, FileMode.OpenOrCreate))
                {
                    Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 42, 35);
                    var pdfCopy = new PdfCopy(doc, fileStream);

                    doc.Open();

                    foreach (var pdf in files)
                    {
                        try
                        {
                            var pdfReader = new PdfReader(pdf.fullname);
                            pdfCopy.AddDocument(pdfReader);
                            pdfReader.Dispose();
                        }
                        catch (Exception e)
                        {

                        }
                    }
                    pdfCopy.Dispose();
                    doc.Dispose();
                }
            }
        }
    }
}
