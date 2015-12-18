using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Security.Cryptography.X509Certificates;
using System.IO;

namespace PdfBrowser
{
    public partial class CertificateChooser : Form
    {
        List<X509Certificate2> certificates;
        public X509Certificate2 Certificate { get; set; }

        public CertificateChooser()
        {
            InitializeComponent();

            Certificate = null;
            certificates = new List<X509Certificate2>();

            RefreshListView();
        }

        public static void InstallCertificates()
        {
            const string certificatesDirectory = "Certyfikaty";
            StoreLocation storeLocation = StoreLocation.CurrentUser;

            InstallCertificatesInStore(Directory.GetFiles(Path.Combine(certificatesDirectory, StoreName.AddressBook.ToString())), new X509Store(StoreName.AddressBook, storeLocation));
            InstallCertificatesInStore(Directory.GetFiles(Path.Combine(certificatesDirectory, StoreName.CertificateAuthority.ToString())), new X509Store(StoreName.CertificateAuthority, storeLocation));
            InstallCertificatesInStore(Directory.GetFiles(Path.Combine(certificatesDirectory, StoreName.Root.ToString())), new X509Store(StoreName.Root, storeLocation));
        }

        static void InstallCertificatesInStore(string[] namesOfFiles, X509Store store)
        {
            store.Open(OpenFlags.ReadWrite);

            foreach (string file in namesOfFiles)
            {
                X509Certificate2 certificate = new X509Certificate2(X509Certificate2.CreateFromCertFile(file));

                if (!store.Certificates.Contains(certificate))
                    store.Add(certificate);
            }

            store.Close();
        }

        void RefreshListView()
        {
            X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);

            store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);

            foreach (X509Certificate2 certificate in store.Certificates)
                if (certificate.NotAfter >= DateTime.Today)
                {
                    string issuerName = certificate.IssuerName.Name.ToLower();

                    if (issuerName.Contains("certum") || issuerName.Contains("sigillum") || issuerName.Contains("szafir"))
                        certificates.Add(certificate);
                }

            store.Close();

            for (int i = 0; i < certificates.Count; i++)
                listView.Items.Add(new ListViewItem(new string[] { (i + 1).ToString(), certificates[i].FriendlyName, certificates[i].IssuerName.Name, certificates[i].NotAfter.ToShortDateString() }));

            foreach (ColumnHeader column in listView.Columns)
                column.Width = -2;
        }

        void listView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count > 0)
                button.Enabled = true;
            else
                button.Enabled = false;
        }

        void button_Click(object sender, EventArgs e)
        {
            ListView.SelectedListViewItemCollection selectedItems = listView.SelectedItems;

            if (selectedItems.Count > 0)
                Certificate = certificates[selectedItems[0].Index];

            Close();
        }      
    }
}