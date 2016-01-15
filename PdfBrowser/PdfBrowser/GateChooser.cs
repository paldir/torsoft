using System;
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

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MainForm.Gate = radioButton1.Checked ? Gate.Test : Gate.Production;

            Close();
        }
    }
}