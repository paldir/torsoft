using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using iTextSharp.text.pdf;
using RGiesecke.DllExport;
using System.Diagnostics;

namespace PdfFiller
{
    public static class Methods
    {
        const System.Runtime.InteropServices.CallingConvention callingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl;

        static void _Fill(string pdfPath, string filledPdfPath, string xmlPath)
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

        static void _Print(string pdfPath)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo(pdfPath);
            processStartInfo.Verb = "print";
            processStartInfo.CreateNoWindow = true;
            processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            Process process = new Process();
            process.StartInfo = processStartInfo;

            process.Start();
        }

        [DllExport("Fill", CallingConvention = callingConvention)]
        public static int Fill(string pdfPath, string filledPdfPath, string xmlPath)
        {
            try
            {
                _Fill(pdfPath, filledPdfPath, xmlPath);

                return 0;
            }
            catch { return -1; }
        }

        [DllExport("FillAndOpen", CallingConvention = callingConvention)]
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

        [DllExport("Print", CallingConvention = callingConvention)]
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