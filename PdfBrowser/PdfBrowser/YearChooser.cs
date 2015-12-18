using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PdfBrowser
{
    public partial class YearChooser : Form
    {
        public YearChooser()
        {
            InitializeComponent();

            _rok.Maximum = DateTime.Now.Year;
            _rok.Value = PdfFile.Rok;
        }

        private void _przycisk_Click(object sender, EventArgs e)
        {
            PdfFile.Rok = Convert.ToInt32(_rok.Value);

            Close();
        }
    }
}