namespace TF.Core.Views
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
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.SubtitleGridView = new TF.Core.Views.GridView.TFDataGridView();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.scintilla1 = new ScintillaNET.Scintilla();
            this.scintilla2 = new ScintillaNET.Scintilla();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.restaurarTextoOriginalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deshacerCambiosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.preTraducirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnImportPo = new System.Windows.Forms.Button();
            this.btnExportPo = new System.Windows.Forms.Button();
            this.lblChangedLinesCount = new System.Windows.Forms.Label();
            this.btnSimpleImport = new System.Windows.Forms.Button();
            this.btnOffsetImport = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.ExportFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.ImportFileDialog = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SubtitleGridView)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dockPanel1
            // 
            this.dockPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dockPanel1.Location = new System.Drawing.Point(0, 0);
            this.dockPanel1.Name = "dockPanel1";
            this.dockPanel1.Size = new System.Drawing.Size(1069, 750);
            this.dockPanel1.TabIndex = 0;
            this.dockPanel1.Theme = null;
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
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.btnImportPo);
            this.splitContainer1.Panel2.Controls.Add(this.btnExportPo);
            this.splitContainer1.Panel2.Controls.Add(this.lblChangedLinesCount);
            this.splitContainer1.Panel2.Controls.Add(this.btnSimpleImport);
            this.splitContainer1.Panel2.Controls.Add(this.btnOffsetImport);
            this.splitContainer1.Panel2.Controls.Add(this.btnExport);
            this.splitContainer1.Size = new System.Drawing.Size(1069, 750);
            this.splitContainer1.SplitterDistance = 716;
            this.splitContainer1.TabIndex = 2;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.SubtitleGridView);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.tableLayoutPanel1);
            this.splitContainer2.Panel2MinSize = 250;
            this.splitContainer2.Size = new System.Drawing.Size(1069, 716);
            this.splitContainer2.SplitterDistance = 459;
            this.splitContainer2.TabIndex = 0;
            // 
            // SubtitleGridView
            // 
            this.SubtitleGridView.AllowUserToAddRows = false;
            this.SubtitleGridView.AllowUserToDeleteRows = false;
            this.SubtitleGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.SubtitleGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SubtitleGridView.Location = new System.Drawing.Point(0, 0);
            this.SubtitleGridView.MultiSelect = false;
            this.SubtitleGridView.Name = "SubtitleGridView";
            this.SubtitleGridView.RowHeadersWidth = 51;
            this.SubtitleGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.SubtitleGridView.Size = new System.Drawing.Size(1069, 459);
            this.SubtitleGridView.TabIndex = 3;
            this.SubtitleGridView.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.SubtitleGridView_CellPainting);
            this.SubtitleGridView.SelectionChanged += new System.EventHandler(this.SubtitleGridView_SelectionChanged);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.scintilla1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.scintilla2, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1069, 253);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // scintilla1
            // 
            this.scintilla1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.scintilla1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scintilla1.Lexer = ScintillaNET.Lexer.Xml;
            this.scintilla1.Location = new System.Drawing.Point(3, 3);
            this.scintilla1.Name = "scintilla1";
            this.scintilla1.Size = new System.Drawing.Size(1063, 120);
            this.scintilla1.TabIndex = 0;
            this.scintilla1.Text = "scintilla1";
            this.scintilla1.ViewEol = true;
            // 
            // scintilla2
            // 
            this.scintilla2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.scintilla2.ContextMenuStrip = this.contextMenuStrip1;
            this.scintilla2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scintilla2.Location = new System.Drawing.Point(3, 129);
            this.scintilla2.Name = "scintilla2";
            this.scintilla2.Size = new System.Drawing.Size(1063, 121);
            this.scintilla2.TabIndex = 1;
            this.scintilla2.Text = "scintilla2";
            this.scintilla2.ViewEol = true;
            this.scintilla2.TextChanged += new System.EventHandler(this.Scintilla2_TextChanged);
            this.scintilla2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Scintilla2_KeyDown);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.restaurarTextoOriginalToolStripMenuItem,
            this.deshacerCambiosToolStripMenuItem,
            this.toolStripSeparator1,
            this.preTraducirToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(196, 76);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.ContextMenuStrip1_Opening);
            // 
            // restaurarTextoOriginalToolStripMenuItem
            // 
            this.restaurarTextoOriginalToolStripMenuItem.Name = "restaurarTextoOriginalToolStripMenuItem";
            this.restaurarTextoOriginalToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.restaurarTextoOriginalToolStripMenuItem.Text = "Restaurar texto original";
            this.restaurarTextoOriginalToolStripMenuItem.Click += new System.EventHandler(this.RestaurarTextoOriginalToolStripMenuItem_Click);
            // 
            // deshacerCambiosToolStripMenuItem
            // 
            this.deshacerCambiosToolStripMenuItem.Name = "deshacerCambiosToolStripMenuItem";
            this.deshacerCambiosToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.deshacerCambiosToolStripMenuItem.Text = "Deshacer cambios";
            this.deshacerCambiosToolStripMenuItem.Click += new System.EventHandler(this.DeshacerCambiosToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(192, 6);
            // 
            // preTraducirToolStripMenuItem
            // 
            this.preTraducirToolStripMenuItem.Enabled = false;
            this.preTraducirToolStripMenuItem.Name = "preTraducirToolStripMenuItem";
            this.preTraducirToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.preTraducirToolStripMenuItem.Text = "Pre-Traducir";
            this.preTraducirToolStripMenuItem.Click += new System.EventHandler(this.PreTraducirToolStripMenuItem_Click);
            // 
            // btnImportPo
            // 
            this.btnImportPo.Location = new System.Drawing.Point(415, 3);
            this.btnImportPo.Name = "btnImportPo";
            this.btnImportPo.Size = new System.Drawing.Size(97, 23);
            this.btnImportPo.TabIndex = 8;
            this.btnImportPo.Text = "Importar de Po";
            this.toolTip1.SetToolTip(this.btnImportPo, "Importa la traducción de un fichero .po");
            this.btnImportPo.UseVisualStyleBackColor = true;
            this.btnImportPo.Click += new System.EventHandler(this.btnImportPo_Click);
            // 
            // btnExportPo
            // 
            this.btnExportPo.Location = new System.Drawing.Point(312, 3);
            this.btnExportPo.Name = "btnExportPo";
            this.btnExportPo.Size = new System.Drawing.Size(97, 23);
            this.btnExportPo.TabIndex = 7;
            this.btnExportPo.Text = "Exportar a Po";
            this.toolTip1.SetToolTip(this.btnExportPo, "Exporta las líneas a un fichero .po");
            this.btnExportPo.UseVisualStyleBackColor = true;
            this.btnExportPo.Click += new System.EventHandler(this.btnExportPo_Click);
            // 
            // lblChangedLinesCount
            // 
            this.lblChangedLinesCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblChangedLinesCount.AutoSize = true;
            this.lblChangedLinesCount.Location = new System.Drawing.Point(581, 8);
            this.lblChangedLinesCount.Name = "lblChangedLinesCount";
            this.lblChangedLinesCount.Size = new System.Drawing.Size(124, 13);
            this.lblChangedLinesCount.TabIndex = 6;
            this.lblChangedLinesCount.Text = "Líneas modificadas: X/Y | Progreso: Z%";
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
            // GridView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1069, 750);
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
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SubtitleGridView)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnSimpleImport;
        private System.Windows.Forms.Button btnOffsetImport;
        private System.Windows.Forms.ToolTip toolTip1;
        protected System.Windows.Forms.SaveFileDialog ExportFileDialog;
        protected System.Windows.Forms.OpenFileDialog ImportFileDialog;
        private System.Windows.Forms.Label lblChangedLinesCount;
        private System.Windows.Forms.SplitContainer splitContainer2;
        protected TFDataGridView SubtitleGridView;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private ScintillaNET.Scintilla scintilla1;
        private ScintillaNET.Scintilla scintilla2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem restaurarTextoOriginalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deshacerCambiosToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem preTraducirToolStripMenuItem;
        private System.Windows.Forms.Button btnImportPo;
        private System.Windows.Forms.Button btnExportPo;
    }
}