using System.Windows.Forms;

namespace PdfBrowser
{
    public sealed partial class ProgressForm : Form
    {
        //ProgressBar progressBar;
        //Label label;
        private readonly string _text;
        private readonly int _countOfProgressIntervals;

        public ProgressForm(string text, int countOfProgressIntervals)
        {
            InitializeComponent();

            _text = Text = text;
            _countOfProgressIntervals = countOfProgressIntervals;
            progressBar.Maximum = countOfProgressIntervals + 1;
            label.Text = text + @" " + progressBar.Value.ToString() + @" z " + countOfProgressIntervals.ToString();
        }

        public void UpdateProgressBar()
        {
            Refresh();

            progressBar.Value++;
            label.Text = _text + @" " + progressBar.Value.ToString() + @" z " + _countOfProgressIntervals.ToString();
        }
    }
}
