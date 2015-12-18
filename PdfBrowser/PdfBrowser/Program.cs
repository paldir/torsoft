using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace PdfBrowser
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            int rok;

            if (args.Length > 0)
                rok = Convert.ToInt32(args[0]);
            else
                rok = DateTime.Now.Year;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm(rok));
        }
    }
}