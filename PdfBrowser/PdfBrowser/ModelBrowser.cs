using System;
using System.Windows.Forms;

using System.IO;

namespace PdfBrowser
{
    public partial class ModelBrowser : Form
    {
        private const string Directory = "wzory_pdf";

        public ModelBrowser()
        {
            InitializeComponent();

            foreach (string modelPath in System.IO.Directory.GetFiles(Directory))
                listView.Items.Add(new ListViewItem(new[] { Path.GetFileName(modelPath) }));

            listView.Columns[0].Width = -1;
        }

        private void listView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            button.Enabled = listView.SelectedItems.Count == 1;
        }

        private void button_Click(object sender, EventArgs e)
        {
            string fileName = listView.SelectedItems[0].Text;
            //string newFileDirectory = Path.Combine(PdfFile.DirectoryName, Path.GetFileNameWithoutExtension(fileName) + "_" + now.Year.ToString() + now.Month.ToString() + now.Day.ToString() + ".pdf");
            string newFileDirectory = Path.Combine(PdfFile.Katalog, Path.GetFileNameWithoutExtension(fileName) + "_" + string.Format("{0:yyyyMMdd}", DateTime.Now) + ".pdf");

            try
            {
                File.Copy(Path.Combine(Directory, fileName), newFileDirectory, true);
                System.Diagnostics.Process.Start(newFileDirectory);
                Close();
            }
            catch (Exception exception) { MessageBox.Show(exception.Message, @"Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
    }
}