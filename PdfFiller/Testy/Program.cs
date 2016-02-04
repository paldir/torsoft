using System;
using System.IO;
using PdfFiller;

namespace Testy
{
    internal class Program
    {
        private static void Main()
        {
            Methods.FillAndOpen("test.pdf", "test_f.pdf", "filler.xml", 0);
        }
    }
}
