using System;
using System.IO;
using System.Linq;
using SharpPDF.Lib;
using Xunit;

namespace SharpPDF.Tests
{
    public class PdfShould
    {     /*
        [Fact]
        public void ReadASimpleObjectTree()
        {
            string pdfFile = @"%PDF-1.1
%¥±ë
1 0 obj << /Type /Catalog /Pages 2 0 R >> endobj
2 0 obj << /Type /Pages /Kids [3 0 R] /Count 1 /MediaBox [0 0 300 144] >> endobj
3 0 obj << /Type /Page /Parent 2 0 R /Resources 
<< /Font 
 << /F1 << /Type /Font /Subtype /Type1 /BaseFont /Times-Roman >> >>
>> /Contents 4 0 R >> endobj
4 0 obj << /Length 39 >> stream
BT /F1 18 Tf 0 0 Td (Hello World) Tj ET
endstream endobj
xref
0 5
0000000000 65535 f 
0000000017 00000 n 
0000000066 00000 n 
0000000147 00000 n 
0000000303 00000 n 
trailer << /Root 1 0 R /Size 5 >>
startxref
392
%%EOF";
            
            byte[] bytes = System.Text.UTF8Encoding.UTF8.GetBytes(pdfFile);
            SharpPdf pdf = new SharpPdf(new MemoryStream(bytes));

            var childs = pdf.GetChilds();
            Assert.Equal(5, childs.ToArray().Count());            
        }*/
    }
}