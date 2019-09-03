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
            this.lblChangedLinesCount = new System.Windows.Forms.Label();
            this.btnSimpleImport = new System.Windows.Forms.Button();
            this.btnOffsetImport = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.ExportFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.ImportFileDialog = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.dockPanel1)).BeginInit();
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
            this.dockPanel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dockPanel1.Name = "dockPanel1";
            this.dockPanel1.Size = new System.Drawing.Size(1425, 923);
            this.dockPanel1.TabIndex = 0;
            this.dockPanel1.Theme = null;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lblChangedLinesCount);
            this.splitContainer1.Panel2.Controls.Add(this.btnSimpleImport);
            this.splitContainer1.Panel2.Controls.Add(this.btnOffsetImport);
            this.splitContainer1.Panel2.Controls.Add(this.btnExport);
            this.splitContainer1.Size = new System.Drawing.Size(1425, 923);
            this.splitContainer1.SplitterDistance = 890;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 2;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
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
            this.splitContainer2.Size = new System.Drawing.Size(1425, 890);
            this.splitContainer2.SplitterDistance = 571;
            this.splitContainer2.SplitterWidth = 5;
            this.splitContainer2.TabIndex = 0;
            // 
            // SubtitleGridView
            // 
            this.SubtitleGridView.AllowUserToAddRows = false;
            this.SubtitleGridView.AllowUserToDeleteRows = false;
            this.SubtitleGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.SubtitleGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SubtitleGridView.Location = new System.Drawing.Point(0, 0);
            this.SubtitleGridView.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.SubtitleGridView.MultiSelect = false;
            this.SubtitleGridView.Name = "SubtitleGridView";
            this.SubtitleGridView.RowHeadersWidth = 51;
            this.SubtitleGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.SubtitleGridView.Size = new System.Drawing.Size(1425, 571);
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
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1425, 314);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // scintilla1
            // 
            this.scintilla1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.scintilla1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scintilla1.Lexer = ScintillaNET.Lexer.Xml;
            this.scintilla1.Location = new System.Drawing.Point(4, 4);
            this.scintilla1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.scintilla1.Name = "scintilla1";
            this.scintilla1.Size = new System.Drawing.Size(1417, 149);
            this.scintilla1.TabIndex = 0;
            this.scintilla1.Text = "scintilla1";
            this.scintilla1.ViewEol = true;
            // 
            // scintilla2
            // 
            this.scintilla2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.scintilla2.ContextMenuStrip = this.contextMenuStrip1;
            this.scintilla2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scintilla2.Location = new System.Drawing.Point(4, 161);
            this.scintilla2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.scintilla2.Name = "scintilla2";
            this.scintilla2.Size = new System.Drawing.Size(1417, 149);
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
            this.contextMenuStrip1.Size = new System.Drawing.Size(234, 82);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.ContextMenuStrip1_Opening);
            // 
            // restaurarTextoOriginalToolStripMenuItem
            // 
            this.restaurarTextoOriginalToolStripMenuItem.Name = "restaurarTextoOriginalToolStripMenuItem";
            this.restaurarTextoOriginalToolStripMenuItem.Size = new System.Drawing.Size(233, 24);
            this.restaurarTextoOriginalToolStripMenuItem.Text = "Restaurar texto original";
            this.restaurarTextoOriginalToolStripMenuItem.Click += new System.EventHandler(this.RestaurarTextoOriginalToolStripMenuItem_Click);
            // 
            // deshacerCambiosToolStripMenuItem
            // 
            this.deshacerCambiosToolStripMenuItem.Name = "deshacerCambiosToolStripMenuItem";
            this.deshacerCambiosToolStripMenuItem.Size = new System.Drawing.Size(233, 24);
            this.deshacerCambiosToolStripMenuItem.Text = "Deshacer cambios";
            this.deshacerCambiosToolStripMenuItem.Click += new System.EventHandler(this.DeshacerCambiosToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(230, 6);
            // 
            // preTraducirToolStripMenuItem
            // 
            this.preTraducirToolStripMenuItem.Enabled = false;
            this.preTraducirToolStripMenuItem.Name = "preTraducirToolStripMenuItem";
            this.preTraducirToolStripMenuItem.Size = new System.Drawing.Size(233, 24);
            this.preTraducirToolStripMenuItem.Text = "Pre-Traducir";
            this.preTraducirToolStripMenuItem.Click += new System.EventHandler(this.PreTraducirToolStripMenuItem_Click);
            // 
            // lblChangedLinesCount
            // 
            this.lblChangedLinesCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblChangedLinesCount.AutoSize = true;
            this.lblChangedLinesCount.Location = new System.Drawing.Point(416, 4);
            this.lblChangedLinesCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblChangedLinesCount.Name = "lblChangedLinesCount";
            this.lblChangedLinesCount.Size = new System.Drawing.Size(159, 17);
            this.lblChangedLinesCount.TabIndex = 6;
            this.lblChangedLinesCount.Text = "Líneas modificadas: X/Y";
            // 
            // btnSimpleImport
            // 
            this.btnSimpleImport.Location = new System.Drawing.Point(279, 4);
            this.btnSimpleImport.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSimpleImport.Name = "btnSimpleImport";
            this.btnSimpleImport.Size = new System.Drawing.Size(129, 28);
            this.btnSimpleImport.TabIndex = 5;
            this.btnSimpleImport.Text = "Importar (Simple)";
            this.toolTip1.SetToolTip(this.btnSimpleImport, "Importa la traducción de un fichero .xlsx\n\nHace coincidir las líneas por el texto" +
        " original.");
            this.btnSimpleImport.UseVisualStyleBackColor = true;
            this.btnSimpleImport.Click += new System.EventHandler(this.btnSimpleImport_Click);
            // 
            // btnOffsetImport
            // 
            this.btnOffsetImport.Location = new System.Drawing.Point(141, 4);
            this.btnOffsetImport.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnOffsetImport.Name = "btnOffsetImport";
            this.btnOffsetImport.Size = new System.Drawing.Size(129, 28);
            this.btnOffsetImport.TabIndex = 4;
            this.btnOffsetImport.Text = "Importar (Offset)";
            this.toolTip1.SetToolTip(this.btnOffsetImport, "Importa la traducción de un fichero .xlsx\n\nHace coincidir las líneas por el campo" +
        " Offset");
            this.btnOffsetImport.UseVisualStyleBackColor = true;
            this.btnOffsetImport.Click += new System.EventHandler(this.btnOffsetImport_Click);
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(4, 4);
            this.btnExport.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(129, 28);
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
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1425, 923);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.dockPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "GridView";
            this.Text = "Subtítulos";
            ((System.ComponentModel.ISupportInitialize)(this.dockPanel1)).EndInit();
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
        private System.Windows.Forms.SaveFileDialog ExportFileDialog;
        private System.Windows.Forms.OpenFileDialog ImportFileDialog;
        private System.Windows.Forms.Label lblChangedLinesCount;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private TFDataGridView SubtitleGridView;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private ScintillaNET.Scintilla scintilla1;
        private ScintillaNET.Scintilla scintilla2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem restaurarTextoOriginalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deshacerCambiosToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem preTraducirToolStripMenuItem;
    }
}