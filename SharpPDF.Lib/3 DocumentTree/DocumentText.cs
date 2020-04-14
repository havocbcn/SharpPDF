using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpPDF.Lib {
    public class DocumentText : IDocumentTree {        
        List<Operator> pageOperators = new List<Operator>();

        public DocumentText(PDFObjects pdf) : base(pdf) {
        }

        public DocumentText(PDFObjects pdf, PdfObject pdfObject)  : base(pdf){            
            var dic = pdf.GetObject<DictionaryObject>(pdfObject);
                
            if (dic.Stream != null)
                this.pageOperators = new PageOperator(dic.Stream).ReadObjects();                    
        } 

        private bool IsLastOperatorATextObject()
            => pageOperators.LastOrDefault() is TextObject;

        private TextObject GetOrAddLastTextObject() {
            var textObject = pageOperators.LastOrDefault() as TextObject;

            if (textObject == null) {
                textObject = new TextObject();
                pageOperators.Add(textObject);
            }

            return textObject;
        }

        public void AddLabel(string text) => GetOrAddLastTextObject().AddLabel(text);

        public void SetPosition(int x, int y) => GetOrAddLastTextObject().SetPosition(x, y);

        public void SetFont(string code, int size) => GetOrAddLastTextObject().SetFont(code, size);

        public void SetLineCap(LineCapStyle lineCap) {
            if (IsLastOperatorATextObject())
                GetOrAddLastTextObject().SetLineCap(lineCap);
            else
                pageOperators.Add(new LineCapOperator(lineCap));
        }

        public void SetNonStrokingColour(float r, float g, float b)
        {
            if (IsLastOperatorATextObject())
                GetOrAddLastTextObject().SetNonStrokingColour(r, g, b);
            else
                pageOperators.Add(new NonStrokingColourOperator(r, g, b));
        }      

        public override void OnSaveEvent(IndirectObject indirectObject)
        {
            string text = string.Join("\n", pageOperators);
            indirectObject.SetChild(new DictionaryObject(text));
        }

        public Operator[] PageOperators => pageOperators.ToArray();


    }
}