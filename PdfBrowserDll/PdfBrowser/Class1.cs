using System;
using System.Collections.Generic;
using System.Text;

using RGiesecke.DllExport;

namespace PdfBrowser
{
    static class Class1
    {
        [DllExport(CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static int OpenPdfBrowser(int rok)
        {
            try { System.Diagnostics.Process.Start("PdfBrowser.exe", rok.ToString()); }
            catch { return -1; }

            return 0;
        }
    }
}