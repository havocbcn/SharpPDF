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
using System.Linq;

namespace crcPdf {
    public class DocumentPageTree : DocumentTree {
        private DocumentPageTree parent;
        public Rectangle MediaBox { get; private set; }
        private readonly List<DocumentPageTree> pageTreeSons = new List<DocumentPageTree>();
        private readonly List<DocumentPage> pageSons = new List<DocumentPage>();

        public override void Load(PDFObjects pdf, PdfObject pdfObject) {
            var dic = pdf.GetObject<DictionaryObject>(pdfObject);
            if (dic.Dictionary.ContainsKey("Parent")) {                
                parent = pdf.GetDocument<DocumentPageTree>(dic.Dictionary["Parent"]);
            }

            var dicKids = pdf.GetObject<ArrayObject>(dic.Dictionary["Kids"]);            
            foreach (var kid in dicKids.Childs<IndirectReferenceObject>()) {
                if (pdf.GetType(kid) == "Pages") {
                    pageTreeSons.Add(pdf.GetDocument<DocumentPageTree>(kid));
                } else {
                    pageSons.Add(pdf.GetDocument<DocumentPage>(kid));
                }
            }

             if (dic.Dictionary.ContainsKey("MediaBox")) {                
                var mediaBox = pdf.GetObject<ArrayObject>(dic.Dictionary["MediaBox"]);
                
                MediaBox = new Rectangle(
                    pdf.GetObject<RealObject>(mediaBox.childs[0]).Value,
                    pdf.GetObject<RealObject>(mediaBox.childs[1]).Value,
                    pdf.GetObject<RealObject>(mediaBox.childs[2]).Value,
                    pdf.GetObject<RealObject>(mediaBox.childs[3]).Value);
            }
        }

        public DocumentPageTree SetMediaBox(Rectangle rectangle) {
            MediaBox = rectangle;
            return this;
        }

        public override void OnSaveEvent(IndirectObject indirectObject, PDFObjects pdfObjects)
        {
            List<PdfObject> kids = new List<PdfObject>();
            kids.AddRange(pageTreeSons.Select(p => p.IndirectReferenceObject(pdfObjects)));
            kids.AddRange(pageSons.Select(p => p.IndirectReferenceObject(pdfObjects)));            

            var entries = new Dictionary<string, PdfObject> {
                { "Type", new NameObject("Pages") },
                { "Kids", new ArrayObject(kids) },
                { "Count", new IntegerObject(kids.Count) },
            };
            
            if (MediaBox != null) {
                entries.Add("MediaBox", new ArrayObject(MediaBox.ToArrayObject()));
            }

            indirectObject.SetChild(new DictionaryObject(entries));
        }

        public DocumentPageTree[] PageTreeSons => pageTreeSons.ToArray();
        public DocumentPage[] PageSons => pageSons.ToArray();
        public DocumentPageTree Parent => parent;
        public DocumentPage AddPage(){
            var page = new DocumentPage(this);
            pageSons.Add(page);
            return page;
        }
    }
}