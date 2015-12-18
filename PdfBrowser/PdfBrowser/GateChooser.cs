using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PdfBrowser
{
    public partial class GateChooser : Form
    {
        public GateChooser()
        {
            InitializeComponent();

            switch (MainForm.Gate)
            {
                case Gate.Test:
                    radioButton1.Checked = true;

                    break;

                case Gate.Production:
                    radioButton2.Checked = true;

                    break;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
                MainForm.Gate = Gate.Test;
            else
                MainForm.Gate = Gate.Production;

            Close();
        }
    }
}