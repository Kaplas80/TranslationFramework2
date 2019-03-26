namespace TFGame.TrailsSky.Files.Exe
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dockPanel1 = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.FontCharsGridView = new System.Windows.Forms.DataGridView();
            this.btnAuto = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dockPanel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FontCharsGridView)).BeginInit();
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
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "hd2_hankaku.dds";
            this.openFileDialog1.Filter = "Fuente Yakuza (*.dds)|*.dds|Todos los archivos (*.*)|*.*";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.FontCharsGridView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.btnAuto);
            this.splitContainer1.Size = new System.Drawing.Size(1496, 645);
            this.splitContainer1.SplitterDistance = 613;
            this.splitContainer1.TabIndex = 3;
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
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FontCharsGridView.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.FontCharsGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.FontCharsGridView.Size = new System.Drawing.Size(1496, 613);
            this.FontCharsGridView.TabIndex = 2;
            this.FontCharsGridView.VirtualMode = true;
            this.FontCharsGridView.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.FontCharsGridView_CellValueNeeded);
            this.FontCharsGridView.CellValuePushed += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.FontCharsGridView_CellValuePushed);
            // 
            // btnAuto
            // 
            this.btnAuto.Location = new System.Drawing.Point(3, 3);
            this.btnAuto.Name = "btnAuto";
            this.btnAuto.Size = new System.Drawing.Size(97, 23);
            this.btnAuto.TabIndex = 3;
            this.btnAuto.Text = "Automático";
            this.btnAuto.UseVisualStyleBackColor = true;
            this.btnAuto.Click += new System.EventHandler(this.btnAuto_Click);
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
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.FontCharsGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView FontCharsGridView;
        private System.Windows.Forms.Button btnAuto;
    }
}