// This file is part of crcPdf.
// 
// crcPdf is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// crcPdf is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with crcPdf.  If not, see <http://www.gnu.org/licenses/>.
using System.Collections.Generic;
using System.IO;

namespace crcPdf {
    // 7.7.2 Document Catalog
    public class DocumentCatalog : IDocumentTree {
        private readonly DocumentPageTree pageTree;
        private readonly DocumentOutline outlines;
        
        public DocumentCatalog(PDFObjects pdf) : base(pdf) {            
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);      
            this.pageTree = new DocumentPageTree(pdf);
        }

        public DocumentCatalog(PDFObjects pdf, PdfObject pdfObject) : base(pdf) {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);      
            var dictionary = pdf.GetObject<DictionaryObject>(pdfObject);
            pageTree = pdf.GetDocument<DocumentPageTree>(dictionary.Dictionary["Pages"]);

            if (dictionary.Dictionary.ContainsKey("Outlines")) {
                outlines = pdf.GetDocument<DocumentOutline>(dictionary.Dictionary["Outlines"]);
            }
        }

        public override void OnSaveEvent(IndirectObject indirectObject)
        {
             var dic = new Dictionary<string, PdfObject> {
                { "Type", new NameObject("Catalog") },
                { "Pages", pageTree.IndirectReferenceObject }
            };

            if (Outlines != null) {
                dic.Add("Outlines", outlines.IndirectReferenceObject);
            }

            indirectObject.SetChild(new DictionaryObject(dic));
        }

        public DocumentPageTree Pages => pageTree;
        public DocumentOutline Outlines => outlines;




        public DocumentCatalog() : base(new PDFObjects()) {                  
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);      
            this.pageTree = new DocumentPageTree(pdfObjects);
        }

        public void Save(Stream ms) 
            => pdfObjects.WriteTo(ms, this, Compression.Compress);        

        public void Save(Stream ms, Compression compression) 
            => pdfObjects.WriteTo(ms, this, compression);

    }
}