namespace TF.Core.Views
{
    partial class ImageView
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
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.imageBox1 = new Cyotek.Windows.Forms.ImageBox();
            this.btnImportImage = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.btnExportImage = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.dockPanel1)).BeginInit();
            this.SuspendLayout();
            // 
            // dockPanel1
            // 
            this.dockPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dockPanel1.Location = new System.Drawing.Point(0, 0);
            this.dockPanel1.Name = "dockPanel1";
            this.dockPanel1.Size = new System.Drawing.Size(800, 402);
            this.dockPanel1.TabIndex = 0;
            this.dockPanel1.Theme = null;
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Right;
            this.propertyGrid1.Location = new System.Drawing.Point(601, 0);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.PropertySort = System.Windows.Forms.PropertySort.NoSort;
            this.propertyGrid1.Size = new System.Drawing.Size(199, 402);
            this.propertyGrid1.TabIndex = 1;
            this.propertyGrid1.ToolbarVisible = false;
            // 
            // imageBox1
            // 
            this.imageBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.imageBox1.BackColor = System.Drawing.Color.Black;
            this.imageBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imageBox1.Location = new System.Drawing.Point(0, 0);
            this.imageBox1.Name = "imageBox1";
            this.imageBox1.Size = new System.Drawing.Size(595, 363);
            this.imageBox1.TabIndex = 4;
            // 
            // btnImportImage
            // 
            this.btnImportImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnImportImage.Location = new System.Drawing.Point(93, 369);
            this.btnImportImage.Name = "btnImportImage";
            this.btnImportImage.Size = new System.Drawing.Size(75, 23);
            this.btnImportImage.TabIndex = 5;
            this.btnImportImage.Text = "Importar";
            this.btnImportImage.UseVisualStyleBackColor = true;
            this.btnImportImage.Click += new System.EventHandler(this.btnImportImage_Click);
            // 
            // btnExportImage
            // 
            this.btnExportImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnExportImage.Location = new System.Drawing.Point(12, 369);
            this.btnExportImage.Name = "btnExportImage";
            this.btnExportImage.Size = new System.Drawing.Size(75, 23);
            this.btnExportImage.TabIndex = 6;
            this.btnExportImage.Text = "Exportar";
            this.btnExportImage.UseVisualStyleBackColor = true;
            this.btnExportImage.Click += new System.EventHandler(this.btnExportImage_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Fondo transparente",
            "Fondo negro"});
            this.comboBox1.Location = new System.Drawing.Point(408, 369);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(187, 21);
            this.comboBox1.TabIndex = 7;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // ImageView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 402);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.btnExportImage);
            this.Controls.Add(this.btnImportImage);
            this.Controls.Add(this.imageBox1);
            this.Controls.Add(this.propertyGrid1);
            this.Controls.Add(this.dockPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ImageView";
            this.Text = "View";
            ((System.ComponentModel.ISupportInitialize)(this.dockPanel1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel1;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private Cyotek.Windows.Forms.ImageBox imageBox1;
        private System.Windows.Forms.Button btnImportImage;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button btnExportImage;
        private System.Windows.Forms.ComboBox comboBox1;
    }
}