using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

using System.IO;
using System.Text;
using System.Xml;
using TestGate = PdfBrowser.pl.gov.edeklaracje.test_bramka;
using System.Diagnostics;
//using iTextSharp.text.pdf;

namespace PdfBrowser
{
    public enum Gate { Test, Production };

    public partial class MainForm : Form
    {
        #region pola
        List<PdfFile> files;
        List<int> indexesOfSelectedItems;
        List<ToolStripMenuItem> buttonsDependentOnOneItem;
        List<ToolStripMenuItem> buttonsDependentOnFewItems;
        List<ToolStripMenuItem> allButtons;
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
            files = new List<PdfFile>();
            allButtons = new List<ToolStripMenuItem>();
            indexesOfSelectedItems = new List<int>();
            buttonsDependentOnOneItem = new List<ToolStripMenuItem>() { openButton, printButton, openXmlButton, openTxtButton, openUpoButton };
            buttonsDependentOnFewItems = new List<ToolStripMenuItem>() { printButton, signButton, downloadUpoButton, deleteButton };

            foreach (ToolStripMenuItem button in buttonsDependentOnOneItem)
                allButtons.Add(button);

            foreach (ToolStripMenuItem button in buttonsDependentOnFewItems)
                allButtons.Add(button);

            statusStrip.Items.Add(ChangeGateButton());
            OdświeżListę();
            listView_ItemSelectionChanged(null, null);
            PrepareEnvironment();
        }
        #endregion

        #region metody
        ToolStripButton ChangeGateButton()
        {
            ToolStripButton changeGateButton = new ToolStripButton("Ustawienia");
            changeGateButton.Click += changeGateButton_Click;

            UstawBramkę();
            UstawRok();

            return changeGateButton;
        }

        void OdświeżListę()
        {
            string[] pathsOfFiles;

            PdfFile.Inicjalizuj();
            listView.Items.Clear();
            files.Clear();
            indexesOfSelectedItems.Clear();

            if (Directory.Exists(PdfFile.Katalog))
                pathsOfFiles = Directory.GetFiles(PdfFile.Katalog, "*.pdf");
            else
                pathsOfFiles = new string[0];

            Array.Sort(pathsOfFiles);

            foreach (string path in pathsOfFiles)
            {
                PdfFile file = new PdfFile(path);

                files.Add(file);
                listView.Items.Add(new ListViewItem(new string[] { String.Empty, file.Id.ToString(), file.NazwaPliku, file.SigIstnieje ? "x" : String.Empty, file.TxtIstnieje ? "x" : String.Empty, file.UpoIstnieje ? "x" : String.Empty }));
            }

            foreach (ColumnHeader column in listView.Columns)
                column.Width = -2;

            if (refreshButton != null)
                foreach (ToolStripMenuItem button in allButtons)
                    button.Enabled = false;
        }

        void UstawBramkę()
        {
            switch (Gate)
            {
                case PdfBrowser.Gate.Production:
                    _rodzajBramki.Text = "BRAMKA RZECZYWISTA";

                    break;

                case PdfBrowser.Gate.Test:
                    _rodzajBramki.Text = "BRAMKA TESTOWA";

                    break;
            }
        }

        void UstawRok()
        {
            yearButton.Text = String.Format("Rok {0}", PdfFile.Rok);
        }

        void ShowMessageBoxOfError(string message) { MessageBox.Show(message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error); }

        void PrepareEnvironment()
        {
            try { new EC2ActiveX.EC2XADESClass(); }
            catch
            {
                try
                {
                    ProcessStartInfo processStartInfo = new ProcessStartInfo("C:\\Windows\\system32\\regsvr32.exe", Path.Combine(Directory.GetCurrentDirectory(), Path.Combine("ec2activex", "ec2activex.dll")));
                    processStartInfo.UseShellExecute = true;
                    processStartInfo.Verb = "runas";

                    try { Process.Start(processStartInfo); }
                    catch
                    {
                        processStartInfo = new ProcessStartInfo("regsvr32", Path.Combine(Directory.GetCurrentDirectory(), Path.Combine("ec2activex", "ec2activex.dll")));
                        processStartInfo.UseShellExecute = true;
                        processStartInfo.Verb = "runas";

                        Process.Start(processStartInfo);
                    }
                }
                catch (Exception exception) { ShowMessageBoxOfError("Błąd instalacji plików potrzebnych do elektronicznego podpisywania dokumentów. " + exception.Message); }
            }

            try { CertificateChooser.InstallCertificates(); }
            catch (Exception exception) { ShowMessageBoxOfError("Błąd instalacji certyfikatów wymaganych przez system e-deklaracje. " + exception.Message); }
        }
        #endregion

        #region zdarzenia
        void listView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e != null)
            {
                ListViewItem item = e.Item;
                int itemIndex = item.Index;

                if (item.Selected)
                    indexesOfSelectedItems.Add(itemIndex);
                else
                    indexesOfSelectedItems.Remove(itemIndex);

                indexesOfSelectedItems.Sort();
            }

            foreach (ToolStripMenuItem button in allButtons)
                button.Enabled = false;

            switch (indexesOfSelectedItems.Count)
            {
                case 0:

                    break;

                case 1:
                    openButton.Enabled = printButton.Enabled = deleteButton.Enabled = true;
                    PdfFile file = files[indexesOfSelectedItems[0]];

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
                    List<PdfFile> checkedFiles = files.FindAll(f => indexesOfSelectedItems.Exists(i => i == f.Id));

                    if (checkedFiles.Exists(f => f.TxtIstnieje))
                        downloadUpoButton.Enabled = true;

                    signButton.Enabled = printButton.Enabled = deleteButton.Enabled = true;

                    break;
            }
        }

        void listView_MouseDoubleClick(object sender, MouseEventArgs e) { openButton_Click(null, null); }

        void openButton_Click(object sender, EventArgs e)
        {
            if (indexesOfSelectedItems.Count > 0)
            {
                PdfFile file = files[indexesOfSelectedItems[0]];

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

                try { Process.Start(file.ŚcieżkaPdf); }
                catch (Exception exception) { ShowMessageBoxOfError(exception.Message); }
            }
        }

        void printButton_Click(object sender, EventArgs e)
        {
            if (indexesOfSelectedItems.Count > 0)
            {
                Enabled = false;

                for (int i = 0; i < indexesOfSelectedItems.Count; i++)
                {
                    PdfFile file = files[indexesOfSelectedItems[i]];

                    try { PdfFiller.Methods.Print(file.ŚcieżkaPdf); }
                    catch { }
                }

                Enabled = true;
            }
        }

        void signButton_Click(object sender, EventArgs e)
        {
            if (indexesOfSelectedItems.Count > 0)
            {
                CertificateChooser certificateChooser = new CertificateChooser();

                certificateChooser.ShowDialog();

                if (certificateChooser.Certificate == null)
                    ShowMessageBoxOfError("Nie wybrano żadnego certyfikatu.");
                else
                {
                    ProgressForm progressForm = new ProgressForm("Podpisywanie i wysyłanie dokumentu", indexesOfSelectedItems.Count);
                    Enabled = false;
                    string certSHA1 = certificateChooser.Certificate.Thumbprint;
                    int okCode = 536870912;
                    StringBuilder report = new StringBuilder();

                    progressForm.Show();

                    using (StringWriter stringWriter = new StringWriter(report))
                    {
                        for (int i = 0; i < indexesOfSelectedItems.Count; i++)
                        {
                            PdfFile file = files[indexesOfSelectedItems[i]];

                            try
                            {
                                EC2ActiveX.EC2XADESClass ec2Xades = new EC2ActiveX.EC2XADESClass();
                                string sigPath = Path.Combine(PdfFile.Katalog, Path.ChangeExtension(file.NazwaPliku, "sig"));
                                string xml = file.EksportujXml();

                                ec2Xades.SetCertificateSHA1(certSHA1);
                                ec2Xades.SetObjectToSign(2, 0, Convert.ToBase64String(Encoding.UTF8.GetBytes(xml)), String.Empty);

                                if (ec2Xades.GetErrorCode == okCode)
                                {
                                    ec2Xades.Sign(0);

                                    if (ec2Xades.GetErrorCode == okCode)
                                    {
                                        string signedXml = Encoding.UTF8.GetString(Convert.FromBase64String(ec2Xades.GetBase64Signed()));

                                        if (ec2Xades.GetErrorCode == okCode)
                                        {
                                            //DateTime today = DateTime.Today;
                                            string todayString = String.Format("{0:yyyy-MM-dd}", DateTime.Today);
                                            string referenceNumber = null;
                                            string statusDescription = null;

                                            using (StreamWriter streamWriter = new StreamWriter(sigPath))
                                                streamWriter.Write(signedXml);

                                            switch (Gate)
                                            {
                                                case PdfBrowser.Gate.Test:
                                                    using (TestGate.GateService gateService = new TestGate.GateService())
                                                    {
                                                        TestGate.sendDocument sendDocument = new TestGate.sendDocument();
                                                        sendDocument.document = Encoding.UTF8.GetBytes(signedXml);
                                                        TestGate.sendDocumentResponse sendDocumentResponse = gateService.sendDocument(sendDocument);
                                                        referenceNumber = sendDocumentResponse.refId;
                                                        statusDescription = sendDocumentResponse.statusOpis;
                                                    }

                                                    break;

                                                case PdfBrowser.Gate.Production:
                                                    using (GateService gateService = new GateService())
                                                    {
                                                        sendDocument sendDocument = new sendDocument();
                                                        sendDocument.document = Encoding.UTF8.GetBytes(signedXml);
                                                        sendDocumentResponse sendDocumentResponse = gateService.sendDocument(sendDocument);
                                                        referenceNumber = sendDocumentResponse.refId;
                                                        statusDescription = sendDocumentResponse.statusOpis;
                                                    }

                                                    break;
                                            }

                                            if (String.IsNullOrEmpty(referenceNumber))
                                                referenceNumber = String.Empty;

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
                            catch (Exception exception) { stringWriter.WriteLine((i + 1).ToString() + ".\t" + file.NazwaPliku + "\tBłąd lokalny: " + exception.Message); }

                            progressForm.UpdateProgressBar();
                        }
                    }

                    string reports = Path.Combine(PdfFile.Katalog, "raporty");
                    string reportPath = Path.Combine(reports, "wysylanie_" + DateTime.Now.ToString().Replace(':', '.') + ".txt");

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
        }

        void openXmlButton_Click(object sender, EventArgs e)
        {
            if (indexesOfSelectedItems.Count > 0)
            {
                PdfFile file = files[indexesOfSelectedItems[0]];

                try { new FileViewer(file.ŚcieżkaSig, FileType.Sig).ShowDialog(); }
                catch (Exception exception) { ShowMessageBoxOfError(exception.Message); }
            }
        }

        void openTxtButton_Click(object sender, EventArgs e)
        {
            if (indexesOfSelectedItems.Count > 0)
            {
                PdfFile file = files[indexesOfSelectedItems[0]];

                try { new FileViewer(file.ŚcieżkiTxt[0], FileType.Txt).ShowDialog(); }
                catch (Exception exception) { ShowMessageBoxOfError(exception.Message); }
            }
        }

        void downloadUpoButton_Click(object sender, EventArgs e)
        {
            if (indexesOfSelectedItems.Count > 0)
            {
                StringBuilder report = new StringBuilder();

                using (StringWriter stringWriter = new StringWriter(report))
                {
                    ProgressForm progressForm = new ProgressForm("Pobieranie UPO", indexesOfSelectedItems.Count);

                    progressForm.Show();

                    for (int i = 0; i < indexesOfSelectedItems.Count; i++)
                    {
                        PdfFile file = files[indexesOfSelectedItems[i]];

                        try
                        {
                            int status = 0;
                            string statusDescription = null;
                            string upo = null;
                            string refId = null;
                            string result = null;

                            switch (Gate)
                            {
                                case PdfBrowser.Gate.Test:
                                    using (TestGate.GateService gateService = new TestGate.GateService())
                                    {
                                        TestGate.requestUPO requestUPO = new TestGate.requestUPO();
                                        requestUPO.refId = file.NumerReferencyjny;
                                        requestUPO.language = TestGate.requestUPOLanguage.pl;
                                        TestGate.requestUPOResponse requestUPOResponseTest = gateService.requestUPO(requestUPO);
                                        status = requestUPOResponseTest.status;
                                        statusDescription = requestUPOResponseTest.statusOpis;
                                        upo = requestUPOResponseTest.upo;
                                        refId = requestUPO.refId;
                                    }

                                    break;

                                case PdfBrowser.Gate.Production:
                                    using (GateService gateService = new GateService())
                                    {
                                        requestUPO requestUPO = new requestUPO();
                                        requestUPO.refId = file.NumerReferencyjny;
                                        requestUPO.language = requestUPOLanguage.pl;
                                        requestUPOResponse requestUPOResponse = gateService.requestUPO(requestUPO);
                                        status = requestUPOResponse.status;
                                        statusDescription = requestUPOResponse.statusOpis;
                                        upo = requestUPOResponse.upo;
                                        refId = requestUPO.refId;
                                    }

                                    break;
                            }

                            if (status <= 199 || status >= 400)
                                result = "Błąd: ";
                            else
                                if (status >= 300 && status <= 399)
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
                        catch (Exception exception) { stringWriter.WriteLine((i + 1).ToString() + ".\t" + file.NazwaPliku + "\tBłąd lokalny: " + exception.Message); }

                        progressForm.UpdateProgressBar();
                    }

                    string reports = Path.Combine(PdfFile.Katalog, "raporty");
                    string reportPath = Path.Combine(reports, "pobieranieUPO_" + DateTime.Now.ToString().Replace(':', '.') + ".txt");

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
        }

        void openUpoButton_Click(object sender, EventArgs e)
        {
            if (indexesOfSelectedItems.Count > 0)
            {
                PdfFile file = files[indexesOfSelectedItems[0]];

                try
                {
                    string upoXml;
                    XmlDocument xmlDocument = new System.Xml.XmlDocument();
                    string upoXmlBufferFile = "upoXmlBuffer.xml";
                    string bufferPath = Path.Combine(PdfFile.Katalog, "buffer");

                    using (StreamReader streamReader = new StreamReader(file.ŚcieżkaUpo))
                        upoXml = streamReader.ReadToEnd();

                    string signingTime = "<" + upoXml.Substring(upoXml.IndexOf("SigningTime"));
                    signingTime = signingTime.Substring(0, signingTime.IndexOf("</")) + "</SigningTime>";
                    upoXml = upoXml.Substring(upoXml.IndexOf("<Potwierdzenie"));
                    upoXml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + upoXml;
                    upoXml = upoXml.Insert(upoXml.IndexOf("</Potwierdzenie>") - 1, "<Stan>4.00000000</Stan>" + signingTime);
                    upoXml = upoXml.Insert(upoXml.IndexOf("<KodUrzedu>") - 1, "<Naglowek><Schema>NONE</Schema>");
                    upoXml = upoXml.Insert(upoXml.IndexOf("</KodUrzedu>") + "</KodUrzedu>".Length, "</Naglowek>");
                    upoXml = upoXml.Substring(0, upoXml.IndexOf("</Potwierdzenie>") + "</Potwierdzenie>".Length);

                    xmlDocument.LoadXml(upoXml);
                    xmlDocument.Save(upoXmlBufferFile);

                    if (!Directory.Exists(bufferPath))
                        Directory.CreateDirectory(bufferPath);

                    if (PdfFiller.Methods.FillAndOpen(Path.Combine("wzory", "UPO_v6-0.pdf"), Path.Combine(bufferPath, "upoPdfBuffer.pdf"), upoXmlBufferFile, 0) != 0)
                        throw new Exception("Błąd otwierania UPO.");

                    if (File.Exists(upoXmlBufferFile))
                        File.Delete(upoXmlBufferFile);
                }
                catch (Exception exception) { ShowMessageBoxOfError(exception.Message); }
            }
        }

        void deleteButton_Click(object sender, EventArgs e)
        {
            if (indexesOfSelectedItems.Count > 0)
                try
                {
                    switch (MessageBox.Show("Czy na pewno chcesz usunąć wybrane pliki?", "Usuwanie", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                    {
                        case System.Windows.Forms.DialogResult.Yes:
                            string deletedPath = Path.Combine(PdfFile.Katalog, "usuniete");

                            if (!Directory.Exists(deletedPath))
                                Directory.CreateDirectory(deletedPath);

                            for (int i = 0; i < indexesOfSelectedItems.Count; i++)
                            {
                                PdfFile file = files[indexesOfSelectedItems[i]];

                                File.Copy(file.ŚcieżkaPdf, Path.Combine(deletedPath, file.NazwaPliku), true);
                                File.Delete(file.ŚcieżkaPdf);

                                if (file.SigIstnieje)
                                {
                                    File.Copy(file.ŚcieżkaSig, Path.Combine(deletedPath, Path.GetFileName(file.ŚcieżkaSig)), true);
                                    File.Delete(file.ŚcieżkaSig);
                                }

                                if (file.TxtIstnieje)
                                    foreach (string txtPath in file.ŚcieżkiTxt)
                                    {
                                        File.Copy(txtPath, Path.Combine(deletedPath, Path.GetFileName(txtPath)), true);
                                        File.Delete(txtPath);
                                    }

                                if (file.UpoIstnieje)
                                {
                                    File.Copy(file.ŚcieżkaUpo, Path.Combine(deletedPath, Path.GetFileName(file.ŚcieżkaUpo)), true);
                                    File.Delete(file.ŚcieżkaUpo);
                                }
                            }

                            OdświeżListę();

                            break;
                    }

                }
                catch (Exception exception) { ShowMessageBoxOfError(exception.Message); }
        }

        void refreshButton_Click(object sender, EventArgs e) { OdświeżListę(); }

        void modelButton_Click(object sender, EventArgs e)
        {
            new ModelBrowser().ShowDialog();
            OdświeżListę();
        }

        void yearButton_Click(object sender, EventArgs e)
        {
            new YearChooser().ShowDialog();
            UstawRok();
            OdświeżListę();
        }

        void changeGateButton_Click(object sender, EventArgs e)
        {
            new GateChooser().ShowDialog();
            UstawBramkę();
            OdświeżListę();
        }
        #endregion
    }
}