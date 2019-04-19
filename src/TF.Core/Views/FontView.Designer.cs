namespace TF.Core.Views
{
    partial class FontView
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
            this.dockPanel1 = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.btnExportImage = new System.Windows.Forms.Button();
            this.btnImportImage = new System.Windows.Forms.Button();
            this.cbFontSize = new System.Windows.Forms.ComboBox();
            this.txtSample = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.dockPanel1)).BeginInit();
            this.SuspendLayout();
            // 
            // dockPanel1
            // 
            this.dockPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dockPanel1.Location = new System.Drawing.Point(0, 0);
            this.dockPanel1.Name = "dockPanel1";
            this.dockPanel1.Size = new System.Drawing.Size(853, 447);
            this.dockPanel1.TabIndex = 0;
            this.dockPanel1.Theme = null;
            // 
            // btnExportImage
            // 
            this.btnExportImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnExportImage.Location = new System.Drawing.Point(12, 412);
            this.btnExportImage.Name = "btnExportImage";
            this.btnExportImage.Size = new System.Drawing.Size(75, 23);
            this.btnExportImage.TabIndex = 8;
            this.btnExportImage.Text = "Exportar";
            this.btnExportImage.UseVisualStyleBackColor = true;
            this.btnExportImage.Click += new System.EventHandler(this.btnExportImage_Click);
            // 
            // btnImportImage
            // 
            this.btnImportImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnImportImage.Location = new System.Drawing.Point(93, 412);
            this.btnImportImage.Name = "btnImportImage";
            this.btnImportImage.Size = new System.Drawing.Size(75, 23);
            this.btnImportImage.TabIndex = 7;
            this.btnImportImage.Text = "Importar";
            this.btnImportImage.UseVisualStyleBackColor = true;
            this.btnImportImage.Click += new System.EventHandler(this.btnImportImage_Click);
            // 
            // cbFontSize
            // 
            this.cbFontSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbFontSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFontSize.FormattingEnabled = true;
            this.cbFontSize.Items.AddRange(new object[] {
            "12",
            "18",
            "24",
            "36",
            "48",
            "60",
            "72"});
            this.cbFontSize.Location = new System.Drawing.Point(736, 414);
            this.cbFontSize.Name = "cbFontSize";
            this.cbFontSize.Size = new System.Drawing.Size(105, 21);
            this.cbFontSize.TabIndex = 9;
            // 
            // txtSample
            // 
            this.txtSample.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSample.Location = new System.Drawing.Point(174, 414);
            this.txtSample.Name = "txtSample";
            this.txtSample.Size = new System.Drawing.Size(556, 20);
            this.txtSample.TabIndex = 10;
            this.txtSample.Text = "El veloz murciélago hindú comía feliz cardillo y kiwi. La cigüeña tocaba el saxof" +
    "ón detrás del palenque de paja.";
            this.txtSample.TextChanged += new System.EventHandler(this.txtSample_TextChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(829, 400);
            this.label1.TabIndex = 11;
            this.label1.Text = "label1";
            // 
            // FontView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(853, 447);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtSample);
            this.Controls.Add(this.cbFontSize);
            this.Controls.Add(this.btnExportImage);
            this.Controls.Add(this.btnImportImage);
            this.Controls.Add(this.dockPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FontView";
            this.Text = "FontView";
            ((System.ComponentModel.ISupportInitialize)(this.dockPanel1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel1;
        private System.Windows.Forms.Button btnExportImage;
        private System.Windows.Forms.Button btnImportImage;
        private System.Windows.Forms.ComboBox cbFontSize;
        private System.Windows.Forms.TextBox txtSample;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    }
}