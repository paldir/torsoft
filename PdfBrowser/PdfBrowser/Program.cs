using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace PdfBrowser
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            int rok = args.Length > 0 ? Convert.ToInt32(args[0]) : DateTime.Now.Year;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm(rok));
        }
    }
}