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
using System;
using System.IO;
using crcPdf.Fonts;

namespace crcPdf {
    public class SimplePdf {
        public DocumentCatalog Catalog { get; private set; }

        private DocumentPage actualPage;

        private readonly Rectangle mediaBox;

        private float textAngle;

        private const float degreesToRadiant = 0.01745329252f;

        public SimplePdf(int width, int height) {                  
            Catalog = new DocumentCatalog();
            mediaBox = new Rectangle(0, 0, width, height);
        }

        public void Save(Stream ms) 
            => Catalog.Save(ms);        

        public void Save(Stream ms, Compression compression) 
            => Catalog.Save(ms, compression);

        public void NewPage() {
            actualPage = Catalog.Pages.AddPage();
            actualPage.SetMediaBox(mediaBox);
        }

        public void DrawImage(byte[] image, float x, float y, float w, float h) {
            actualPage.SaveGraph();
            actualPage.CurrentTransformationMatrix(w, 0, 0, h, x, y);
            actualPage.AddImage(image);
            actualPage.RestoreGraph();
        }

        /// <summary>
        /// Set current font
        /// </summary>
        /// <param name="font">A font</param>
        /// <param name="size">Size</param>
        public void SetFont(DocumentFont font, int size) {
            actualPage.SetFont(font, size);
        }

        public void GetFont(string name, bool IsBold, bool IsItalic, Embedded embedded)
            => FontFactory.GetFont(name, IsBold, IsItalic, embedded);

        /// <summary>
        /// Set a rotation for text in degrees
        /// </summary>
        /// <param name="textAngle">Degrees</param>
        public void TextAngle(float textAngle) {
            this.textAngle = textAngle;
        }

        public void DrawText(string text, float x, float y) {
            if (textAngle > -0.0001 && textAngle < 0.0001) {            
                actualPage.SetTextPositioning(x, y);
            } else {
                float sinus = (float)Math.Sin(textAngle * degreesToRadiant);
				float cosinus = (float)Math.Cos(textAngle * degreesToRadiant);
                //float fontHeight = (actualPage.CurrentFont.GetDescendent - actualPage.CurrentFont.GetHeight) * pageSize.GetDPI;
				//x = x - sinus * fontHeight;
				//y = y + cosinus * fontHeight - fontHeight;
				actualPage.SetTextMatrix(cosinus, sinus, -sinus, cosinus, x, y);
                
            }
            actualPage.AddLabel(text);
        }        

        public void SetColor(float r, float g, float b) 
            => actualPage.AddNonStrokingColour(r, g, b);

        public void DrawRectangle(float x, float y, float width, float height)  {
            actualPage.AddRectangle(x, y, width, height);
            actualPage.AddStroke();
        }

         public void DrawRectangleFull(float x, float y, float width, float height)  {
            actualPage.AddRectangle(x, y, width, height);
            actualPage.AddFill();
            actualPage.AddStroke();
        }
            
        
    }
}