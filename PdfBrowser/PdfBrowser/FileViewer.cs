using System;
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
        //private string _filePath;
        //private FileType _fileType;
        private readonly string _bufferedFilePath;

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
                case FileType.Txt:
                    break;

                default:
                    throw new ArgumentOutOfRangeException("fileType", fileType, null);
            }

            string file;

            using (StreamReader streamReader = new StreamReader(filePath))
                file = streamReader.ReadToEnd();

            switch (fileType)
            {
                case FileType.Sig:
                case FileType.Upo:
                    string formattedXml;

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

                case FileType.Txt:
                    break;

                case FileType.SigningReport:
                    break;

                case FileType.ReportOfUpoDownloading:
                    break;

                default:
                    throw new ArgumentOutOfRangeException("fileType", fileType, null);
            }

            //_fileType = fileType;
            //_filePath = filePath;
            textBox.Text = file;
            string bufferPath = Path.Combine(PdfFile.Katalog, "buffer");

            if (!Directory.Exists(bufferPath))
                Directory.CreateDirectory(bufferPath);
            else
                foreach (string fileName in Directory.GetFiles(bufferPath, "*.txt"))
                    File.Delete(fileName);

            using (StreamWriter streamWriter = new StreamWriter(_bufferedFilePath = Path.Combine(bufferPath, Path.GetFileNameWithoutExtension(filePath) + ".txt")))
                streamWriter.Write(file);
        }

        private void printButton_Click(object sender, EventArgs e)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo(_bufferedFilePath) {Verb = "print"};

            Process.Start(processStartInfo);
        }
    }
}