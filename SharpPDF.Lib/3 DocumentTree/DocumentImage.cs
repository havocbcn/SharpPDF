// This file is part of SharpPdf.
// 
// SharpPdf is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// SharpPdf is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with SharpPdf.  If not, see <http://www.gnu.org/licenses/>.
namespace SharpPDF.Lib {
    public abstract class DocumentImage : IDocumentTree {

        protected DocumentImage(PDFObjects pdf) : base(pdf) {
        }

        protected DocumentImage(PDFObjects pdf, PdfObject pdfObject) : base(pdf) {             
        }

        public abstract int Width { get; }
        public abstract int Height { get; }
    }
}