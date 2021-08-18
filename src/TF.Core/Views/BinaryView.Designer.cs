namespace TF.Core.Views
{
    partial class BinaryView
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
            this.btnImportFile = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.btnExportFile = new System.Windows.Forms.Button();
            this.hexBox1 = new Be.Windows.Forms.HexBox();
            this.SuspendLayout();
            // 
            // dockPanel1
            // 
            this.dockPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dockPanel1.Location = new System.Drawing.Point(0, 0);
            this.dockPanel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dockPanel1.Name = "dockPanel1";
            this.dockPanel1.Size = new System.Drawing.Size(1067, 586);
            this.dockPanel1.TabIndex = 0;
            this.dockPanel1.Theme = null;
            // 
            // btnImportFile
            // 
            this.btnImportFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnImportFile.Location = new System.Drawing.Point(124, 545);
            this.btnImportFile.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnImportFile.Name = "btnImportFile";
            this.btnImportFile.Size = new System.Drawing.Size(100, 28);
            this.btnImportFile.TabIndex = 5;
            this.btnImportFile.Text = "Importar";
            this.btnImportFile.UseVisualStyleBackColor = true;
            this.btnImportFile.Click += new System.EventHandler(this.btnImportFile_Click);
            // 
            // btnExportFile
            // 
            this.btnExportFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnExportFile.Location = new System.Drawing.Point(16, 545);
            this.btnExportFile.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnExportFile.Name = "btnExportFile";
            this.btnExportFile.Size = new System.Drawing.Size(100, 28);
            this.btnExportFile.TabIndex = 6;
            this.btnExportFile.Text = "Exportar";
            this.btnExportFile.UseVisualStyleBackColor = true;
            this.btnExportFile.Click += new System.EventHandler(this.btnExportFile_Click);
            // 
            // hexBox1
            // 
            this.hexBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hexBox1.ColumnInfoVisible = true;
            this.hexBox1.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hexBox1.GroupSeparatorVisible = true;
            this.hexBox1.LineInfoVisible = true;
            this.hexBox1.Location = new System.Drawing.Point(0, 0);
            this.hexBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.hexBox1.Name = "hexBox1";
            this.hexBox1.ReadOnly = true;
            this.hexBox1.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
            this.hexBox1.Size = new System.Drawing.Size(1067, 537);
            this.hexBox1.StringViewVisible = true;
            this.hexBox1.TabIndex = 7;
            this.hexBox1.UseFixedBytesPerLine = true;
            this.hexBox1.VScrollBarVisible = true;
            // 
            // BinaryView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 586);
            this.Controls.Add(this.hexBox1);
            this.Controls.Add(this.btnExportFile);
            this.Controls.Add(this.btnImportFile);
            this.Controls.Add(this.dockPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "BinaryView";
            this.Text = "View";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BinaryView_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel1;
        private System.Windows.Forms.Button btnImportFile;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button btnExportFile;
        private Be.Windows.Forms.HexBox hexBox1;
    }
}