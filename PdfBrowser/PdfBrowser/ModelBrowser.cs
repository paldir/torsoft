using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.IO;

namespace PdfBrowser
{
    public partial class ModelBrowser : Form
    {
        const string directory = "wzory_pdf";

        public ModelBrowser()
        {
            InitializeComponent();

            foreach (string modelPath in Directory.GetFiles(directory))
                listView.Items.Add(new ListViewItem(new string[] { Path.GetFileName(modelPath) }));

            listView.Columns[0].Width = -1;
        }

        void listView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (listView.SelectedItems.Count == 1)
                button.Enabled = true;
            else
                button.Enabled = false;
        }

        void button_Click(object sender, EventArgs e)
        {
            string fileName = listView.SelectedItems[0].Text;
            //string newFileDirectory = Path.Combine(PdfFile.DirectoryName, Path.GetFileNameWithoutExtension(fileName) + "_" + now.Year.ToString() + now.Month.ToString() + now.Day.ToString() + ".pdf");
            string newFileDirectory = Path.Combine(PdfFile.Katalog, Path.GetFileNameWithoutExtension(fileName) + "_" + String.Format("{0:yyyyMMdd}", DateTime.Now) + ".pdf");

            try
            {
                File.Copy(Path.Combine(directory, fileName), newFileDirectory, true);
                System.Diagnostics.Process.Start(newFileDirectory);
                Close();
            }
            catch (Exception exception) { MessageBox.Show(exception.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
    }
}