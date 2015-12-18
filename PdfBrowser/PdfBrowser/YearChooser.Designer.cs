namespace PdfBrowser
{
    partial class YearChooser
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(YearChooser));
            this._rok = new System.Windows.Forms.NumericUpDown();
            this._przycisk = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this._rok)).BeginInit();
            this.SuspendLayout();
            // 
            // _rok
            // 
            this._rok.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this._rok.Location = new System.Drawing.Point(12, 28);
            this._rok.Maximum = new decimal(new int[] {
            2014,
            0,
            0,
            0});
            this._rok.Minimum = new decimal(new int[] {
            2014,
            0,
            0,
            0});
            this._rok.Name = "_rok";
            this._rok.Size = new System.Drawing.Size(120, 22);
            this._rok.TabIndex = 0;
            this._rok.Value = new decimal(new int[] {
            2014,
            0,
            0,
            0});
            // 
            // _przycisk
            // 
            this._przycisk.Location = new System.Drawing.Point(36, 56);
            this._przycisk.Name = "_przycisk";
            this._przycisk.Size = new System.Drawing.Size(75, 23);
            this._przycisk.TabIndex = 1;
            this._przycisk.Text = "OK";
            this._przycisk.UseVisualStyleBackColor = true;
            this._przycisk.Click += new System.EventHandler(this._przycisk_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "Wybierz rok:";
            // 
            // YearChooser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(144, 91);
            this.Controls.Add(this.label1);
            this.Controls.Add(this._przycisk);
            this.Controls.Add(this._rok);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "YearChooser";
            this.Text = "Wybór roku";
            ((System.ComponentModel.ISupportInitialize)(this._rok)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown _rok;
        private System.Windows.Forms.Button _przycisk;
        private System.Windows.Forms.Label label1;
    }
}