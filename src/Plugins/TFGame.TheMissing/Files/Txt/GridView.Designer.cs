namespace TFGame.TheMissing.Files.Txt
{
    partial class GridView
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
            this.components = new System.ComponentModel.Container();
            this.dockPanel1 = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.SubtitleGridView = new TFGame.TheMissing.Files.Txt.GridView.TFDataGridView();
            this.lblChangedLinesCount = new System.Windows.Forms.Label();
            this.btnSimpleImport = new System.Windows.Forms.Button();
            this.btnOffsetImport = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.ExportFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.ImportFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.btnAutoAdjust = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SubtitleGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // dockPanel1
            // 
            this.dockPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dockPanel1.Location = new System.Drawing.Point(0, 0);
            this.dockPanel1.Name = "dockPanel1";
            this.dockPanel1.Size = new System.Drawing.Size(1069, 450);
            this.dockPanel1.TabIndex = 0;
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
            this.splitContainer1.Panel1.Controls.Add(this.SubtitleGridView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.btnAutoAdjust);
            this.splitContainer1.Panel2.Controls.Add(this.lblChangedLinesCount);
            this.splitContainer1.Panel2.Controls.Add(this.btnSimpleImport);
            this.splitContainer1.Panel2.Controls.Add(this.btnOffsetImport);
            this.splitContainer1.Panel2.Controls.Add(this.btnExport);
            this.splitContainer1.Size = new System.Drawing.Size(1069, 450);
            this.splitContainer1.SplitterDistance = 418;
            this.splitContainer1.TabIndex = 2;
            // 
            // SubtitleGridView
            // 
            this.SubtitleGridView.AllowUserToAddRows = false;
            this.SubtitleGridView.AllowUserToDeleteRows = false;
            this.SubtitleGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.SubtitleGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SubtitleGridView.Location = new System.Drawing.Point(0, 0);
            this.SubtitleGridView.Name = "SubtitleGridView";
            this.SubtitleGridView.Size = new System.Drawing.Size(1069, 418);
            this.SubtitleGridView.TabIndex = 0;
            this.SubtitleGridView.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.SubtitleGridView_CellBeginEdit);
            this.SubtitleGridView.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.SubtitleGridView_CellEndEdit);
            this.SubtitleGridView.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.SubtitleGridView_CellMouseDoubleClick);
            this.SubtitleGridView.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.SubtitleGridView_CellPainting);
            this.SubtitleGridView.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.SubtitleGridView_EditingControlShowing);
            // 
            // lblChangedLinesCount
            // 
            this.lblChangedLinesCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblChangedLinesCount.AutoSize = true;
            this.lblChangedLinesCount.Location = new System.Drawing.Point(312, 8);
            this.lblChangedLinesCount.Name = "lblChangedLinesCount";
            this.lblChangedLinesCount.Size = new System.Drawing.Size(124, 13);
            this.lblChangedLinesCount.TabIndex = 6;
            this.lblChangedLinesCount.Text = "Líneas modificadas: X/Y";
            // 
            // btnSimpleImport
            // 
            this.btnSimpleImport.Location = new System.Drawing.Point(209, 3);
            this.btnSimpleImport.Name = "btnSimpleImport";
            this.btnSimpleImport.Size = new System.Drawing.Size(97, 23);
            this.btnSimpleImport.TabIndex = 5;
            this.btnSimpleImport.Text = "Importar (Simple)";
            this.toolTip1.SetToolTip(this.btnSimpleImport, "Importa la traducción de un fichero .xlsx\n\nHace coincidir las líneas por el texto" +
        " original.");
            this.btnSimpleImport.UseVisualStyleBackColor = true;
            this.btnSimpleImport.Click += new System.EventHandler(this.btnSimpleImport_Click);
            // 
            // btnOffsetImport
            // 
            this.btnOffsetImport.Location = new System.Drawing.Point(106, 3);
            this.btnOffsetImport.Name = "btnOffsetImport";
            this.btnOffsetImport.Size = new System.Drawing.Size(97, 23);
            this.btnOffsetImport.TabIndex = 4;
            this.btnOffsetImport.Text = "Importar (Offset)";
            this.toolTip1.SetToolTip(this.btnOffsetImport, "Importa la traducción de un fichero .xlsx\n\nHace coincidir las líneas por el campo" +
        " Offset");
            this.btnOffsetImport.UseVisualStyleBackColor = true;
            this.btnOffsetImport.Click += new System.EventHandler(this.btnOffsetImport_Click);
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(3, 3);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(97, 23);
            this.btnExport.TabIndex = 3;
            this.btnExport.Text = "Exportar a Excel";
            this.toolTip1.SetToolTip(this.btnExport, "Exporta las líneas a un fichero .xlsx");
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // ExportFileDialog
            // 
            this.ExportFileDialog.Filter = "Archivos Excel|*.xlsx";
            // 
            // ImportFileDialog
            // 
            this.ImportFileDialog.Filter = "Archivos Excel|*.xlsx";
            // 
            // btnAutoAdjust
            // 
            this.btnAutoAdjust.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAutoAdjust.Location = new System.Drawing.Point(969, 3);
            this.btnAutoAdjust.Name = "btnAutoAdjust";
            this.btnAutoAdjust.Size = new System.Drawing.Size(97, 23);
            this.btnAutoAdjust.TabIndex = 7;
            this.btnAutoAdjust.Text = "Auto Ajustar";
            this.toolTip1.SetToolTip(this.btnAutoAdjust, "Exporta las líneas a un fichero .xlsx");
            this.btnAutoAdjust.UseVisualStyleBackColor = true;
            this.btnAutoAdjust.Click += new System.EventHandler(this.btnAutoAdjust_Click);
            // 
            // GridView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1069, 450);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.dockPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "GridView";
            this.Text = "Subtítulos";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SubtitleGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnSimpleImport;
        private System.Windows.Forms.Button btnOffsetImport;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.SaveFileDialog ExportFileDialog;
        private System.Windows.Forms.OpenFileDialog ImportFileDialog;
        private System.Windows.Forms.Label lblChangedLinesCount;
        private TFGame.TheMissing.Files.Txt.GridView.TFDataGridView SubtitleGridView;
        private System.Windows.Forms.Button btnAutoAdjust;
    }
}