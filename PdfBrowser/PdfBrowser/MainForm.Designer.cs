namespace PdfBrowser
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.openButton = new System.Windows.Forms.ToolStripMenuItem();
            this.printButton = new System.Windows.Forms.ToolStripMenuItem();
            this.signButton = new System.Windows.Forms.ToolStripMenuItem();
            this.openXmlButton = new System.Windows.Forms.ToolStripMenuItem();
            this.openTxtButton = new System.Windows.Forms.ToolStripMenuItem();
            this.downloadUpoButton = new System.Windows.Forms.ToolStripMenuItem();
            this.openUpoButton = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteButton = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshButton = new System.Windows.Forms.ToolStripMenuItem();
            this.modelButton = new System.Windows.Forms.ToolStripMenuItem();
            this.yearButton = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this._rodzajBramki = new System.Windows.Forms.ToolStripStatusLabel();
            this.listView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.menuStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openButton,
            this.printButton,
            this.signButton,
            this.openXmlButton,
            this.openTxtButton,
            this.downloadUpoButton,
            this.openUpoButton,
            this.deleteButton,
            this.refreshButton,
            this.modelButton,
            this.yearButton});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(884, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip1";
            // 
            // openButton
            // 
            this.openButton.Name = "openButton";
            this.openButton.Size = new System.Drawing.Size(57, 20);
            this.openButton.Text = "Otwórz";
            this.openButton.Click += new System.EventHandler(this.openButton_Click);
            // 
            // printButton
            // 
            this.printButton.Name = "printButton";
            this.printButton.Size = new System.Drawing.Size(54, 20);
            this.printButton.Text = "Drukuj";
            this.printButton.Click += new System.EventHandler(this.printButton_Click);
            // 
            // signButton
            // 
            this.signButton.Name = "signButton";
            this.signButton.Size = new System.Drawing.Size(98, 20);
            this.signButton.Text = "Podpisz i wyślij";
            this.signButton.Click += new System.EventHandler(this.signButton_Click);
            // 
            // openXmlButton
            // 
            this.openXmlButton.Name = "openXmlButton";
            this.openXmlButton.Size = new System.Drawing.Size(77, 20);
            this.openXmlButton.Text = "Pokaż XML";
            this.openXmlButton.Click += new System.EventHandler(this.openXmlButton_Click);
            // 
            // openTxtButton
            // 
            this.openTxtButton.Name = "openTxtButton";
            this.openTxtButton.Size = new System.Drawing.Size(156, 20);
            this.openTxtButton.Text = "Pokaż numer referencyjny";
            this.openTxtButton.Click += new System.EventHandler(this.openTxtButton_Click);
            // 
            // downloadUpoButton
            // 
            this.downloadUpoButton.Name = "downloadUpoButton";
            this.downloadUpoButton.Size = new System.Drawing.Size(85, 20);
            this.downloadUpoButton.Text = "Pobierz UPO";
            this.downloadUpoButton.Click += new System.EventHandler(this.downloadUpoButton_Click);
            // 
            // openUpoButton
            // 
            this.openUpoButton.Name = "openUpoButton";
            this.openUpoButton.Size = new System.Drawing.Size(77, 20);
            this.openUpoButton.Text = "Pokaż UPO";
            this.openUpoButton.Click += new System.EventHandler(this.openUpoButton_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(46, 20);
            this.deleteButton.Text = "Usuń";
            this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
            // 
            // refreshButton
            // 
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(63, 20);
            this.refreshButton.Text = "Odśwież";
            this.refreshButton.Click += new System.EventHandler(this.refreshButton_Click);
            // 
            // modelButton
            // 
            this.modelButton.Name = "modelButton";
            this.modelButton.Size = new System.Drawing.Size(52, 20);
            this.modelButton.Text = "Wzory";
            this.modelButton.Click += new System.EventHandler(this.modelButton_Click);
            // 
            // yearButton
            // 
            this.yearButton.Name = "yearButton";
            this.yearButton.Size = new System.Drawing.Size(66, 20);
            this.yearButton.Text = "Rok 2015";
            this.yearButton.Click += new System.EventHandler(this.yearButton_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._rodzajBramki});
            this.statusStrip.Location = new System.Drawing.Point(0, 614);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(884, 22);
            this.statusStrip.TabIndex = 1;
            this.statusStrip.Text = "statusStrip";
            // 
            // _rodzajBramki
            // 
            this._rodzajBramki.Name = "_rodzajBramki";
            this._rodzajBramki.Size = new System.Drawing.Size(79, 17);
            this._rodzajBramki.Text = "rodzaj bramki";
            // 
            // listView
            // 
            this.listView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7});
            this.listView.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.listView.FullRowSelect = true;
            this.listView.GridLines = true;
            this.listView.HideSelection = false;
            this.listView.Location = new System.Drawing.Point(12, 27);
            this.listView.Name = "listView";
            this.listView.Size = new System.Drawing.Size(860, 584);
            this.listView.TabIndex = 2;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = System.Windows.Forms.View.Details;
            this.listView.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.listView_ItemSelectionChanged);
            this.listView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listView_MouseDoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "";
            this.columnHeader1.Width = 1;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "L.p.";
            this.columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Nazwa pliku";
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "XML";
            this.columnHeader4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Numer referencyjny";
            this.columnHeader5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "UPO";
            this.columnHeader6.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 636);
            this.Controls.Add(this.listView);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.Name = "MainForm";
            this.Text = "Przeglądarka PDF";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem openButton;
        private System.Windows.Forms.ToolStripMenuItem signButton;
        private System.Windows.Forms.ToolStripMenuItem openXmlButton;
        private System.Windows.Forms.ToolStripMenuItem openTxtButton;
        private System.Windows.Forms.ToolStripMenuItem downloadUpoButton;
        private System.Windows.Forms.ToolStripMenuItem openUpoButton;
        private System.Windows.Forms.ToolStripMenuItem deleteButton;
        private System.Windows.Forms.ToolStripMenuItem refreshButton;
        private System.Windows.Forms.ToolStripMenuItem modelButton;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel _rodzajBramki;
        private System.Windows.Forms.ListView listView;
        private System.Windows.Forms.ToolStripMenuItem printButton;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ToolStripMenuItem yearButton;
    }
}

