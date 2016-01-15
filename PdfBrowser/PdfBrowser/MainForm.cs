using System;
using System.Collections.Generic;
using System.Windows.Forms;

using System.IO;
using System.Text;
using System.Xml;
using TestGate = PdfBrowser.pl.gov.edeklaracje.test_bramka;
using System.Diagnostics;
using System.Globalization;

//using iTextSharp.text.pdf;

namespace PdfBrowser
{
    public enum Gate { Test, Production };

    public partial class MainForm : Form
    {
        #region pola

        private readonly List<PdfFile> _files;
        private readonly List<int> _indexesOfSelectedItems;
        //private readonly List<ToolStripMenuItem> _buttonsDependentOnOneItem;
        //private readonly List<ToolStripMenuItem> _buttonsDependentOnFewItems;
        private readonly List<ToolStripMenuItem> _allButtons;
        #endregion

        #region właściwości
        public static Gate Gate { get; set; }
        #endregion

        #region konstruktory
        static MainForm() { Gate = Gate.Production; }

        public MainForm(int rok)
        {
            InitializeComponent();

            PdfFile.Rok = rok;
            _files = new List<PdfFile>();
            _allButtons = new List<ToolStripMenuItem>();
            _indexesOfSelectedItems = new List<int>();
            List<ToolStripMenuItem> buttonsDependentOnOneItem = new List<ToolStripMenuItem>() { openButton, printButton, openXmlButton, openTxtButton, openUpoButton };
            List<ToolStripMenuItem> buttonsDependentOnFewItems = new List<ToolStripMenuItem>() { printButton, signButton, downloadUpoButton, deleteButton };

            foreach (ToolStripMenuItem button in buttonsDependentOnOneItem)
                _allButtons.Add(button);

            foreach (ToolStripMenuItem button in buttonsDependentOnFewItems)
                _allButtons.Add(button);

            statusStrip.Items.Add(ChangeGateButton());
            OdświeżListę();
            listView_ItemSelectionChanged(null, null);
            PrepareEnvironment();
        }
        #endregion

        #region metody

        private ToolStripButton ChangeGateButton()
        {
            ToolStripButton changeGateButton = new ToolStripButton("Ustawienia");
            changeGateButton.Click += changeGateButton_Click;

            UstawBramkę();
            UstawRok();

            return changeGateButton;
        }

        private void OdświeżListę()
        {
            PdfFile.Inicjalizuj();
            listView.Items.Clear();
            _files.Clear();
            _indexesOfSelectedItems.Clear();

            string[] pathsOfFiles = Directory.Exists(PdfFile.Katalog) ? Directory.GetFiles(PdfFile.Katalog, "*.pdf") : new string[0];

            Array.Sort(pathsOfFiles);

            foreach (string path in pathsOfFiles)
            {
                PdfFile file = new PdfFile(path);

                _files.Add(file);
                listView.Items.Add(new ListViewItem(new[] { string.Empty, file.Id.ToString(), file.NazwaPliku, file.SigIstnieje ? "x" : string.Empty, file.TxtIstnieje ? "x" : string.Empty, file.UpoIstnieje ? "x" : string.Empty }));
            }

            foreach (ColumnHeader column in listView.Columns)
                column.Width = -2;

            if (refreshButton == null) return;

            foreach (ToolStripMenuItem button in _allButtons)
                button.Enabled = false;
        }

        private void UstawBramkę()
        {
            switch (Gate)
            {
                case Gate.Production:
                    _rodzajBramki.Text = @"BRAMKA RZECZYWISTA";

                    break;

                case Gate.Test:
                    _rodzajBramki.Text = @"BRAMKA TESTOWA";

                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void UstawRok()
        {
            yearButton.Text = string.Format("Rok {0}", PdfFile.Rok);
        }

        private static void ShowMessageBoxOfError(string message)
        {
            MessageBox.Show(message, @"Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private static void PrepareEnvironment()
        {
            try
            {
                new EC2ActiveX.EC2XADESClass();
            }
            catch
            {
                try
                {
                    ProcessStartInfo processStartInfo = new ProcessStartInfo("C:\\Windows\\system32\\regsvr32.exe", Path.Combine(Directory.GetCurrentDirectory(), Path.Combine("ec2activex", "ec2activex.dll")))
                    {
                        UseShellExecute = true,
                        Verb = "runas"
                    };

                    try
                    {
                        Process.Start(processStartInfo);
                    }
                    catch
                    {
                        processStartInfo = new ProcessStartInfo("regsvr32", Path.Combine(Directory.GetCurrentDirectory(), Path.Combine("ec2activex", "ec2activex.dll")))
                        {
                            UseShellExecute = true,
                            Verb = "runas"
                        };

                        Process.Start(processStartInfo);
                    }
                }
                catch (Exception exception)
                {
                    ShowMessageBoxOfError("Błąd instalacji plików potrzebnych do elektronicznego podpisywania dokumentów. " + exception.Message);
                }
            }

            try
            {
                CertificateChooser.InstallCertificates();
            }
            catch (Exception exception)
            {
                ShowMessageBoxOfError("Błąd instalacji certyfikatów wymaganych przez system e-deklaracje. " + exception.Message);
            }
        }

        #endregion

        #region zdarzenia

        private void listView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e != null)
            {
                ListViewItem item = e.Item;
                int itemIndex = item.Index;

                if (item.Selected)
                    _indexesOfSelectedItems.Add(itemIndex);
                else
                    _indexesOfSelectedItems.Remove(itemIndex);

                _indexesOfSelectedItems.Sort();
            }

            foreach (ToolStripMenuItem button in _allButtons)
                button.Enabled = false;

            switch (_indexesOfSelectedItems.Count)
            {
                case 0:

                    break;

                case 1:
                    openButton.Enabled = printButton.Enabled = deleteButton.Enabled = true;
                    PdfFile file = _files[_indexesOfSelectedItems[0]];

                    signButton.Enabled = true;

                    if (file.SigIstnieje)
                        openXmlButton.Enabled = true;

                    if (file.TxtIstnieje)
                    {
                        openTxtButton.Enabled = true;
                        downloadUpoButton.Enabled = true;
                    }

                    if (file.UpoIstnieje)
                        openUpoButton.Enabled = true;

                    break;

                default:
                    List<PdfFile> checkedFiles = _files.FindAll(f => _indexesOfSelectedItems.Exists(i => i == f.Id));

                    if (checkedFiles.Exists(f => f.TxtIstnieje))
                        downloadUpoButton.Enabled = true;

                    signButton.Enabled = printButton.Enabled = deleteButton.Enabled = true;

                    break;
            }
        }

        private void listView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            openButton_Click(null, null);
        }

        private void openButton_Click(object sender, EventArgs e)
        {
            if (_indexesOfSelectedItems.Count <= 0) return;

            PdfFile file = _files[_indexesOfSelectedItems[0]];

            //PdfFiller.Methods.FillAndOpen(file.PdfPath, "C:\\test.pdf", "C:\\test.xml", 0);

            /*using (FileStream pdf = new FileStream(file.PdfPath, FileMode.Open))
                using (FileStream filledPdf = new FileStream("C:\\test.pdf", FileMode.Create))
                {
                    PdfReader.unethicalreading = true;

                    using (PdfReader pdfReader = new PdfReader(pdf))
                    {
                        using (PdfStamper stamper = new PdfStamper(pdfReader, filledPdf, '\0', true))
                        {
                            PdfContentByte pdfContentByte = stamper.GetOverContent(1);
                            iTextSharp.text.Rectangle pageSize = pdfReader.GetPageSize(1);
                            iTextSharp.text.Rectangle cropBox = pdfReader.GetCropBox(1);

                            if (cropBox != null && (cropBox.Width < pageSize.Width || cropBox.Height < cropBox.Height))
                                pageSize = cropBox;
                            
                            pdfContentByte.BeginText();
                            pdfContentByte.SetColorFill(new iTextSharp.text.BaseColor(Color.Black));
                            pdfContentByte.SetColorStroke(new iTextSharp.text.BaseColor(Color.Black));
                            pdfContentByte.SetFontAndSize(BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.WINANSI, BaseFont.EMBEDDED), 50);
                            pdfContentByte.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "BLA BLA BLA BLA", pageSize.Left + 100, pageSize.Top - 500, 0);
                            pdfContentByte.EndText();
                        }
                    }
                }*/

            try
            {
                Process.Start(file.ŚcieżkaPdf);
            }
            catch (Exception exception)
            {
                ShowMessageBoxOfError(exception.Message);
            }
        }

        private void printButton_Click(object sender, EventArgs e)
        {
            if (_indexesOfSelectedItems.Count <= 0) return;

            Enabled = false;

            for (int i = 0; i < _indexesOfSelectedItems.Count; i++)
            {
                PdfFile file = _files[_indexesOfSelectedItems[i]];

                try
                {
                    PdfFiller.Methods.Print(file.ŚcieżkaPdf);
                }
                catch
                {
                }
            }

            Enabled = true;
        }

        private void signButton_Click(object sender, EventArgs e)
        {
            if (_indexesOfSelectedItems.Count <= 0) return;

            CertificateChooser certificateChooser = new CertificateChooser();

            certificateChooser.ShowDialog();

            if (certificateChooser.Certificate == null)
                ShowMessageBoxOfError("Nie wybrano żadnego certyfikatu.");
            else
            {
                ProgressForm progressForm = new ProgressForm("Podpisywanie i wysyłanie dokumentu", _indexesOfSelectedItems.Count);
                Enabled = false;
                string certSha1 = certificateChooser.Certificate.Thumbprint;
                const int okCode = 536870912;
                StringBuilder report = new StringBuilder();

                progressForm.Show();

                using (StringWriter stringWriter = new StringWriter(report))
                {
                    for (int i = 0; i < _indexesOfSelectedItems.Count; i++)
                    {
                        PdfFile file = _files[_indexesOfSelectedItems[i]];

                        try
                        {
                            EC2ActiveX.EC2XADESClass ec2Xades = new EC2ActiveX.EC2XADESClass();
                            string sigPath = Path.Combine(PdfFile.Katalog, Path.ChangeExtension(file.NazwaPliku, "sig"));
                            string xml = file.EksportujXml();

                            ec2Xades.SetCertificateSHA1(certSha1);
                            ec2Xades.SetObjectToSign(2, 0, Convert.ToBase64String(Encoding.UTF8.GetBytes(xml)), string.Empty);

                            if (ec2Xades.GetErrorCode == okCode)
                            {
                                ec2Xades.Sign(0);

                                if (ec2Xades.GetErrorCode == okCode)
                                {
                                    string signedXml = Encoding.UTF8.GetString(Convert.FromBase64String(ec2Xades.GetBase64Signed()));

                                    if (ec2Xades.GetErrorCode == okCode)
                                    {
                                        //DateTime today = DateTime.Today;
                                        string todayString = string.Format("{0:yyyy-MM-dd}", DateTime.Today);
                                        string referenceNumber;
                                        string statusDescription;

                                        using (StreamWriter streamWriter = new StreamWriter(sigPath))
                                            streamWriter.Write(signedXml);

                                        switch (Gate)
                                        {
                                            case Gate.Test:
                                                using (TestGate.GateService gateService = new TestGate.GateService())
                                                {
                                                    TestGate.sendDocument sendDocument = new TestGate.sendDocument {document = Encoding.UTF8.GetBytes(signedXml)};
                                                    TestGate.sendDocumentResponse sendDocumentResponse = gateService.sendDocument(sendDocument);
                                                    referenceNumber = sendDocumentResponse.refId;
                                                    statusDescription = sendDocumentResponse.statusOpis;
                                                }

                                                break;

                                            case Gate.Production:
                                                using (GateService gateService = new GateService())
                                                {
                                                    sendDocument sendDocument = new sendDocument {document = Encoding.UTF8.GetBytes(signedXml)};
                                                    sendDocumentResponse sendDocumentResponse = gateService.sendDocument(sendDocument);
                                                    referenceNumber = sendDocumentResponse.refId;
                                                    statusDescription = sendDocumentResponse.statusOpis;
                                                }

                                                break;

                                            default:
                                                throw new ArgumentOutOfRangeException();
                                        }

                                        if (string.IsNullOrEmpty(referenceNumber))
                                            referenceNumber = string.Empty;

                                        stringWriter.WriteLine((i + 1).ToString() + ".\t" + file.NazwaPliku + "\t" + statusDescription + "\tNumer referencyjny: " + referenceNumber);

                                        using (StreamWriter streamWriter = new StreamWriter(Path.Combine(PdfFile.Katalog, Path.GetFileNameWithoutExtension(file.NazwaPliku) + "_" + todayString + ".txt")))
                                        {
                                            streamWriter.WriteLine("Numer referencyjny: " + referenceNumber);
                                            streamWriter.WriteLine("Data wysłania: " + todayString);
                                            streamWriter.WriteLine("Dotyczy: " + file.NazwaPliku);
                                        }
                                    }
                                    else
                                        throw new Exception("Błąd pobierania podpisanego pliku XML. " + ec2Xades.GetErrorDescription);
                                }
                                else
                                    throw new Exception("Błąd podpisywania pliku XML. " + ec2Xades.GetErrorDescription);
                            }
                            else
                                throw new Exception("Błąd pliku XML wyeksportowanego z dokumentu PDF. " + ec2Xades.GetErrorDescription);
                        }
                        catch (Exception exception)
                        {
                            stringWriter.WriteLine(i + 1 + ".\t" + file.NazwaPliku + "\tBłąd lokalny: " + exception.Message);
                        }

                        progressForm.UpdateProgressBar();
                    }
                }

                string reports = Path.Combine(PdfFile.Katalog, "raporty");
                string reportPath = Path.Combine(reports, "wysylanie_" + DateTime.Now.ToString(CultureInfo.InvariantCulture).Replace(':', '.') + ".txt");

                if (!Directory.Exists(reports))
                    Directory.CreateDirectory(reports);

                using (StreamWriter streamWriter = new StreamWriter(reportPath))
                    streamWriter.Write(report);

                FileViewer fileViewer = new FileViewer(reportPath, FileType.SigningReport);
                Enabled = true;

                OdświeżListę();
                progressForm.Close();
                fileViewer.ShowDialog();
            }
        }

        private void openXmlButton_Click(object sender, EventArgs e)
        {
            if (_indexesOfSelectedItems.Count <= 0) return;

            PdfFile file = _files[_indexesOfSelectedItems[0]];

            try
            {
                new FileViewer(file.ŚcieżkaSig, FileType.Sig).ShowDialog();
            }
            catch (Exception exception)
            {
                ShowMessageBoxOfError(exception.Message);
            }
        }

        private void openTxtButton_Click(object sender, EventArgs e)
        {
            if (_indexesOfSelectedItems.Count <= 0) return;

            PdfFile file = _files[_indexesOfSelectedItems[0]];

            try
            {
                new FileViewer(file.ŚcieżkiTxt[0], FileType.Txt).ShowDialog();
            }
            catch (Exception exception)
            {
                ShowMessageBoxOfError(exception.Message);
            }
        }

        private void downloadUpoButton_Click(object sender, EventArgs e)
        {
            if (_indexesOfSelectedItems.Count <= 0) return;

            StringBuilder report = new StringBuilder();

            using (StringWriter stringWriter = new StringWriter(report))
            {
                ProgressForm progressForm = new ProgressForm("Pobieranie UPO", _indexesOfSelectedItems.Count);

                progressForm.Show();

                for (int i = 0; i < _indexesOfSelectedItems.Count; i++)
                {
                    PdfFile file = _files[_indexesOfSelectedItems[i]];

                    try
                    {
                        int status;
                        string statusDescription;
                        string upo;
                        string refId;
                        string result;

                        switch (Gate)
                        {
                            case Gate.Test:
                                using (TestGate.GateService gateService = new TestGate.GateService())
                                {
                                    TestGate.requestUPO requestUpo = new TestGate.requestUPO
                                    {
                                        refId = file.NumerReferencyjny,
                                        language = TestGate.requestUPOLanguage.pl
                                    };

                                    TestGate.requestUPOResponse requestUpoResponseTest = gateService.requestUPO(requestUpo);
                                    status = requestUpoResponseTest.status;
                                    statusDescription = requestUpoResponseTest.statusOpis;
                                    upo = requestUpoResponseTest.upo;
                                    refId = requestUpo.refId;
                                }

                                break;

                            case Gate.Production:
                                using (GateService gateService = new GateService())
                                {
                                    requestUPO requestUpo = new requestUPO
                                    {
                                        refId = file.NumerReferencyjny,
                                        language = requestUPOLanguage.pl
                                    };

                                    requestUPOResponse requestUpoResponse = gateService.requestUPO(requestUpo);
                                    status = requestUpoResponse.status;
                                    statusDescription = requestUpoResponse.statusOpis;
                                    upo = requestUpoResponse.upo;
                                    refId = requestUpo.refId;
                                }

                                break;

                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                        if ((status <= 199) || (status >= 400))
                            result = "Błąd: ";
                        else if ((status >= 300) && (status <= 399))
                            result = "Ostrzeżenie: ";
                        else
                        {
                            result = "Informacja: ";
                            XmlDocument xmlUpo = new XmlDocument();

                            xmlUpo.LoadXml(upo);
                            xmlUpo.Save(Path.Combine(PdfFile.Katalog, "UPO_" + refId + ".xml.xades"));
                        }

                        stringWriter.WriteLine((i + 1).ToString() + ".\t" + file.NazwaPliku + "\t" + result + statusDescription);
                    }
                    catch (Exception exception)
                    {
                        stringWriter.WriteLine((i + 1).ToString() + ".\t" + file.NazwaPliku + "\tBłąd lokalny: " + exception.Message);
                    }

                    progressForm.UpdateProgressBar();
                }

                string reports = Path.Combine(PdfFile.Katalog, "raporty");
                string reportPath = Path.Combine(reports, "pobieranieUPO_" + DateTime.Now.ToString(CultureInfo.InvariantCulture).Replace(':', '.') + ".txt");

                if (!Directory.Exists(reports))
                    Directory.CreateDirectory(reports);

                using (StreamWriter streamWriter = new StreamWriter(reportPath))
                    streamWriter.Write(report);

                FileViewer fileViewer = new FileViewer(reportPath, FileType.ReportOfUpoDownloading);
                Enabled = true;

                OdświeżListę();
                progressForm.Close();
                fileViewer.ShowDialog();
            }
        }

        private void openUpoButton_Click(object sender, EventArgs e)
        {
            if (_indexesOfSelectedItems.Count <= 0) return;

            PdfFile file = _files[_indexesOfSelectedItems[0]];

            try
            {
                string upoXml;
                XmlDocument xmlDocument = new XmlDocument();
                const string upoXmlBufferFile = "upoXmlBuffer.xml";
                string bufferPath = Path.Combine(PdfFile.Katalog, "buffer");

                using (StreamReader streamReader = new StreamReader(file.ŚcieżkaUpo))
                    upoXml = streamReader.ReadToEnd();

                string signingTime = "<" + upoXml.Substring(upoXml.IndexOf("SigningTime", StringComparison.Ordinal));
                signingTime = signingTime.Substring(0, signingTime.IndexOf("</", StringComparison.Ordinal)) + "</SigningTime>";
                upoXml = upoXml.Substring(upoXml.IndexOf("<Potwierdzenie", StringComparison.Ordinal));
                upoXml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + upoXml;
                upoXml = upoXml.Insert(upoXml.IndexOf("</Potwierdzenie>", StringComparison.Ordinal) - 1, "<Stan>4.00000000</Stan>" + signingTime);
                upoXml = upoXml.Insert(upoXml.IndexOf("<KodUrzedu>", StringComparison.Ordinal) - 1, "<Naglowek><Schema>NONE</Schema>");
                upoXml = upoXml.Insert(upoXml.IndexOf("</KodUrzedu>", StringComparison.Ordinal) + "</KodUrzedu>".Length, "</Naglowek>");
                upoXml = upoXml.Substring(0, upoXml.IndexOf("</Potwierdzenie>", StringComparison.Ordinal) + "</Potwierdzenie>".Length);

                xmlDocument.LoadXml(upoXml);
                xmlDocument.Save(upoXmlBufferFile);

                if (!Directory.Exists(bufferPath))
                    Directory.CreateDirectory(bufferPath);

                if (PdfFiller.Methods.FillAndOpen(Path.Combine("wzory", "UPO_v6-0.pdf"), Path.Combine(bufferPath, "upoPdfBuffer.pdf"), upoXmlBufferFile, 0) != 0)
                    throw new Exception("Błąd otwierania UPO.");

                if (File.Exists(upoXmlBufferFile))
                    File.Delete(upoXmlBufferFile);
            }
            catch (Exception exception)
            {
                ShowMessageBoxOfError(exception.Message);
            }
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (_indexesOfSelectedItems.Count <= 0) return;

            try
            {
                switch (MessageBox.Show(@"Czy na pewno chcesz usunąć wybrane pliki?", @"Usuwanie", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    case DialogResult.Yes:
                        string deletedPath = Path.Combine(PdfFile.Katalog, "usuniete");

                        if (!Directory.Exists(deletedPath))
                            Directory.CreateDirectory(deletedPath);

                        for (int i = 0; i < _indexesOfSelectedItems.Count; i++)
                        {
                            PdfFile file = _files[_indexesOfSelectedItems[i]];

                            File.Copy(file.ŚcieżkaPdf, Path.Combine(deletedPath, file.NazwaPliku), true);
                            File.Delete(file.ŚcieżkaPdf);

                            if (file.SigIstnieje)
                            {
                                string ścieżkaSig = file.ŚcieżkaSig;

                                if (!string.IsNullOrEmpty(ścieżkaSig))
                                {
                                    File.Copy(ścieżkaSig, Path.Combine(deletedPath, Path.GetFileName(ścieżkaSig)), true);
                                    File.Delete(ścieżkaSig);
                                }
                            }

                            if (file.TxtIstnieje)
                                foreach (string txtPath in file.ŚcieżkiTxt)
                                    if (!string.IsNullOrEmpty(txtPath))
                                    {
                                        File.Copy(txtPath, Path.Combine(deletedPath, Path.GetFileName(txtPath)), true);
                                        File.Delete(txtPath);
                                    }

                            if (!file.UpoIstnieje) continue;

                            string ścieżkaUpo = file.ŚcieżkaUpo;

                            if (string.IsNullOrEmpty(ścieżkaUpo)) continue;

                            File.Copy(ścieżkaUpo, Path.Combine(deletedPath, Path.GetFileName(ścieżkaUpo)), true);
                            File.Delete(ścieżkaUpo);
                        }

                        OdświeżListę();

                        break;

                    case DialogResult.None:
                        break;

                    case DialogResult.OK:
                        break;

                    case DialogResult.Cancel:
                        break;

                    case DialogResult.Abort:
                        break;

                    case DialogResult.Retry:
                        break;

                    case DialogResult.Ignore:
                        break;

                    case DialogResult.No:
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch (Exception exception)
            {
                ShowMessageBoxOfError(exception.Message);
            }
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            OdświeżListę();
        }

        private void modelButton_Click(object sender, EventArgs e)
        {
            new ModelBrowser().ShowDialog();
            OdświeżListę();
        }

        private void yearButton_Click(object sender, EventArgs e)
        {
            new YearChooser().ShowDialog();
            UstawRok();
            OdświeżListę();
        }

        private void changeGateButton_Click(object sender, EventArgs e)
        {
            new GateChooser().ShowDialog();
            UstawBramkę();
            OdświeżListę();
        }

        #endregion
    }
}