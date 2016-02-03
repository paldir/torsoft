using System.IO;
using iTextSharp.text.pdf;
using RGiesecke.DllExport;
using System.Diagnostics;

namespace PdfFiller
{
    public static class Methods
    {
        private const System.Runtime.InteropServices.CallingConvention CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl;

        private static void _Fill(string pdfPath, string filledPdfPath, string xmlPath)
        {
            using (FileStream pdf = new FileStream(pdfPath, FileMode.Open))
            using (FileStream xml = new FileStream(xmlPath, FileMode.Open))
            using (FileStream filledPdf = new FileStream(filledPdfPath, FileMode.Create))
            {
                PdfReader.unethicalreading = true;

                using (PdfReader pdfReader = new PdfReader(pdf))
                using (PdfStamper stamper = new PdfStamper(pdfReader, filledPdf, '\0', true))
                    stamper.AcroFields.Xfa.FillXfaForm(xml);
            }
        }

        private static void _Print(string pdfPath)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo(pdfPath)
            {
                Verb = "print",
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden
            };
            Process process = new Process {StartInfo = processStartInfo};

            process.Start();
        }

        [DllExport("Fill", CallingConvention = CallingConvention)]
        public static int Fill(string pdfPath, string filledPdfPath, string xmlPath)
        {
            try
            {
                _Fill(pdfPath, filledPdfPath, xmlPath);

                return 0;
            }
            catch { return -1; }
        }

        [DllExport("FillAndOpen", CallingConvention = CallingConvention)]
        public static int FillAndOpen(string pdfPath, string filledPdfPath, string xmlPath, int print)
        {
            try
            {
                _Fill(pdfPath, filledPdfPath, xmlPath);

                if (print == 1)
                    _Print(filledPdfPath);
                else
                    Process.Start(filledPdfPath);

                return 0;
            }
            catch { return -1; }
        }

        [DllExport("Print", CallingConvention = CallingConvention)]
        public static int Print(string pdfPath)
        {
            try
            {
                _Print(pdfPath);

                return 0;
            }
            catch { return -1; }
        }
    }
}