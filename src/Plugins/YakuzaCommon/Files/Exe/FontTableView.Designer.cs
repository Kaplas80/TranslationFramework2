namespace YakuzaCommon.Files.Exe
{
    partial class FontTableView
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.FontCharsGridView = new System.Windows.Forms.DataGridView();
            this.btnAutoAdjust = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.txtTest = new System.Windows.Forms.TextBox();
            this.imgBoxNewChar = new Cyotek.Windows.Forms.ImageBox();
            this.btnLoadNewFont = new System.Windows.Forms.Button();
            this.imgBoxOriginalChar = new Cyotek.Windows.Forms.ImageBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.btnLoadOriginalFont = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.dockPanel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FontCharsGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // dockPanel1
            // 
            this.dockPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dockPanel1.Location = new System.Drawing.Point(0, 0);
            this.dockPanel1.Name = "dockPanel1";
            this.dockPanel1.Size = new System.Drawing.Size(1496, 645);
            this.dockPanel1.TabIndex = 0;
            this.dockPanel1.Theme = null;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.FontCharsGridView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.btnAutoAdjust);
            this.splitContainer1.Panel2.Controls.Add(this.pictureBox1);
            this.splitContainer1.Panel2.Controls.Add(this.txtTest);
            this.splitContainer1.Panel2.Controls.Add(this.imgBoxNewChar);
            this.splitContainer1.Panel2.Controls.Add(this.btnLoadNewFont);
            this.splitContainer1.Panel2.Controls.Add(this.imgBoxOriginalChar);
            this.splitContainer1.Panel2.Controls.Add(this.pictureBox2);
            this.splitContainer1.Panel2.Controls.Add(this.btnLoadOriginalFont);
            this.splitContainer1.Size = new System.Drawing.Size(1496, 645);
            this.splitContainer1.SplitterDistance = 726;
            this.splitContainer1.TabIndex = 1;
            // 
            // FontCharsGridView
            // 
            this.FontCharsGridView.AllowUserToAddRows = false;
            this.FontCharsGridView.AllowUserToDeleteRows = false;
            this.FontCharsGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.FontCharsGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FontCharsGridView.Location = new System.Drawing.Point(0, 0);
            this.FontCharsGridView.MultiSelect = false;
            this.FontCharsGridView.Name = "FontCharsGridView";
            this.FontCharsGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.FontCharsGridView.Size = new System.Drawing.Size(726, 645);
            this.FontCharsGridView.TabIndex = 2;
            this.FontCharsGridView.VirtualMode = true;
            this.FontCharsGridView.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.FontCharsGridView_CellEndEdit);
            this.FontCharsGridView.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.FontCharsGridView_CellValueNeeded);
            this.FontCharsGridView.CellValuePushed += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.FontCharsGridView_CellValuePushed);
            this.FontCharsGridView.SelectionChanged += new System.EventHandler(this.FontCharsGridView_SelectionChanged);
            // 
            // btnAutoAdjust
            // 
            this.btnAutoAdjust.Location = new System.Drawing.Point(528, 12);
            this.btnAutoAdjust.Name = "btnAutoAdjust";
            this.btnAutoAdjust.Size = new System.Drawing.Size(75, 23);
            this.btnAutoAdjust.TabIndex = 11;
            this.btnAutoAdjust.Text = "Auto Ajustar";
            this.btnAutoAdjust.UseVisualStyleBackColor = true;
            this.btnAutoAdjust.Click += new System.EventHandler(this.btnAutoAdjust_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BackColor = System.Drawing.Color.Black;
            this.pictureBox1.Location = new System.Drawing.Point(3, 383);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(763, 128);
            this.pictureBox1.TabIndex = 10;
            this.pictureBox1.TabStop = false;
            // 
            // txtTest
            // 
            this.txtTest.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTest.Location = new System.Drawing.Point(0, 357);
            this.txtTest.Name = "txtTest";
            this.txtTest.Size = new System.Drawing.Size(763, 20);
            this.txtTest.TabIndex = 9;
            this.txtTest.Text = "Texto de prueba";
            this.txtTest.TextChanged += new System.EventHandler(this.txtTest_TextChanged);
            // 
            // imgBoxNewChar
            // 
            this.imgBoxNewChar.AllowFreePan = false;
            this.imgBoxNewChar.AllowZoom = false;
            this.imgBoxNewChar.AutoPan = false;
            this.imgBoxNewChar.BackColor = System.Drawing.Color.Black;
            this.imgBoxNewChar.GridDisplayMode = Cyotek.Windows.Forms.ImageBoxGridDisplayMode.None;
            this.imgBoxNewChar.Location = new System.Drawing.Point(393, 41);
            this.imgBoxNewChar.Name = "imgBoxNewChar";
            this.imgBoxNewChar.PanMode = Cyotek.Windows.Forms.ImageBoxPanMode.Middle;
            this.imgBoxNewChar.PixelGridColor = System.Drawing.Color.DarkRed;
            this.imgBoxNewChar.PixelGridThreshold = 1;
            this.imgBoxNewChar.ShowPixelGrid = true;
            this.imgBoxNewChar.Size = new System.Drawing.Size(384, 512);
            this.imgBoxNewChar.SizeMode = Cyotek.Windows.Forms.ImageBoxSizeMode.Fit;
            this.imgBoxNewChar.TabIndex = 8;
            this.imgBoxNewChar.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            // 
            // btnLoadNewFont
            // 
            this.btnLoadNewFont.Location = new System.Drawing.Point(393, 12);
            this.btnLoadNewFont.Name = "btnLoadNewFont";
            this.btnLoadNewFont.Size = new System.Drawing.Size(129, 23);
            this.btnLoadNewFont.TabIndex = 7;
            this.btnLoadNewFont.Text = "Cargar Fuente Nueva";
            this.btnLoadNewFont.UseVisualStyleBackColor = true;
            this.btnLoadNewFont.Click += new System.EventHandler(this.btnLoadFont_Click);
            // 
            // imgBoxOriginalChar
            // 
            this.imgBoxOriginalChar.AllowFreePan = false;
            this.imgBoxOriginalChar.AllowZoom = false;
            this.imgBoxOriginalChar.AutoPan = false;
            this.imgBoxOriginalChar.BackColor = System.Drawing.Color.Black;
            this.imgBoxOriginalChar.GridDisplayMode = Cyotek.Windows.Forms.ImageBoxGridDisplayMode.None;
            this.imgBoxOriginalChar.Location = new System.Drawing.Point(3, 41);
            this.imgBoxOriginalChar.Name = "imgBoxOriginalChar";
            this.imgBoxOriginalChar.PanMode = Cyotek.Windows.Forms.ImageBoxPanMode.Middle;
            this.imgBoxOriginalChar.PixelGridColor = System.Drawing.Color.DarkRed;
            this.imgBoxOriginalChar.PixelGridThreshold = 1;
            this.imgBoxOriginalChar.ShowPixelGrid = true;
            this.imgBoxOriginalChar.Size = new System.Drawing.Size(384, 512);
            this.imgBoxOriginalChar.SizeMode = Cyotek.Windows.Forms.ImageBoxSizeMode.Fit;
            this.imgBoxOriginalChar.TabIndex = 6;
            this.imgBoxOriginalChar.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox2.BackColor = System.Drawing.Color.Black;
            this.pictureBox2.Location = new System.Drawing.Point(3, 517);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(763, 128);
            this.pictureBox2.TabIndex = 5;
            this.pictureBox2.TabStop = false;
            // 
            // btnLoadOriginalFont
            // 
            this.btnLoadOriginalFont.Location = new System.Drawing.Point(3, 12);
            this.btnLoadOriginalFont.Name = "btnLoadOriginalFont";
            this.btnLoadOriginalFont.Size = new System.Drawing.Size(129, 23);
            this.btnLoadOriginalFont.TabIndex = 0;
            this.btnLoadOriginalFont.Text = "Cargar Fuente Original";
            this.btnLoadOriginalFont.UseVisualStyleBackColor = true;
            this.btnLoadOriginalFont.Click += new System.EventHandler(this.btnLoadFont_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "hd2_hankaku.dds";
            this.openFileDialog1.Filter = "Fuente Yakuza (*.dds)|*.dds|Todos los archivos (*.*)|*.*";
            // 
            // FontTableView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1496, 645);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.dockPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FontTableView";
            this.Text = "Fuente";
            ((System.ComponentModel.ISupportInitialize)(this.dockPanel1)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.FontCharsGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView FontCharsGridView;
        private System.Windows.Forms.Button btnLoadOriginalFont;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private Cyotek.Windows.Forms.ImageBox imgBoxOriginalChar;
        private Cyotek.Windows.Forms.ImageBox imgBoxNewChar;
        private System.Windows.Forms.Button btnLoadNewFont;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox txtTest;
        private System.Windows.Forms.Button btnAutoAdjust;
    }
}