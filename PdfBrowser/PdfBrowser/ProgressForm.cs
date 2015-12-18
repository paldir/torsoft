using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PdfBrowser
{
    public partial class ProgressForm : Form
    {
        //ProgressBar progressBar;
        //Label label;
        string text;
        int countOfProgressIntervals;

        public ProgressForm(string text, int countOfProgressIntervals)
        {
            InitializeComponent();

            this.text = Text = text;
            this.countOfProgressIntervals = countOfProgressIntervals;
            progressBar.Maximum = countOfProgressIntervals + 1;
            label.Text = text + " " + progressBar.Value.ToString() + " z " + countOfProgressIntervals.ToString();
        }

        public void UpdateProgressBar()
        {
            Refresh();

            progressBar.Value++;
            label.Text = text + " " + progressBar.Value.ToString() + " z " + countOfProgressIntervals.ToString();
        }
    }
}
