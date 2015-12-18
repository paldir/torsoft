using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.IO;
using System.Xml;
using System.Diagnostics;

namespace PdfBrowser
{
    public enum FileType
    {
        Sig,
        Txt,
        Upo,
        SigningReport,
        ReportOfUpoDownloading
    };

    public partial class FileViewer : Form
    {
        string file;
        string filePath;
        FileType fileType;
        string bufferedFilePath;

        public FileViewer(string filePath, FileType fileType)
        {
            InitializeComponent();

            switch (fileType)
            {
                case FileType.Sig:
                case FileType.Upo:
                case FileType.SigningReport:
                case FileType.ReportOfUpoDownloading:
                    WindowState = FormWindowState.Maximized;

                    break;
            }

            using (StreamReader streamReader = new StreamReader(filePath))
                file = streamReader.ReadToEnd();

            switch (fileType)
            {
                case FileType.Sig:
                case FileType.Upo:
                    string formattedXml = String.Empty;

                    using (MemoryStream memoryStream = new MemoryStream())
                    using (XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, System.Text.Encoding.Unicode))
                    {
                        XmlDocument xmlDocument = new XmlDocument();
                        xmlTextWriter.Formatting = Formatting.Indented;

                        xmlDocument.LoadXml(file);
                        xmlDocument.WriteContentTo(xmlTextWriter);
                        xmlTextWriter.Flush();
                        memoryStream.Flush();

                        memoryStream.Position = 0;

                        using (StreamReader nestedStreamReader = new StreamReader(memoryStream))
                            formattedXml = nestedStreamReader.ReadToEnd();
                    }

                    file = formattedXml;

                    break;
            }

            this.fileType = fileType;
            this.filePath = filePath;
            textBox.Text = file;
            string bufferPath = Path.Combine(PdfFile.Katalog, "buffer");

            if (!Directory.Exists(bufferPath))
                Directory.CreateDirectory(bufferPath);
            else
                foreach (string fileName in Directory.GetFiles(bufferPath, "*.txt"))
                    File.Delete(fileName);

            using (StreamWriter streamWriter = new StreamWriter(bufferedFilePath = Path.Combine(bufferPath, Path.GetFileNameWithoutExtension(filePath) + ".txt")))
                streamWriter.Write(file);
        }

        void printButton_Click(object sender, EventArgs e)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo(bufferedFilePath);
            processStartInfo.Verb = "print";

            Process.Start(processStartInfo);
        }
    }
}