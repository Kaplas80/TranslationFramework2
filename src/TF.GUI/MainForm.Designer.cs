namespace TF.GUI
{
    partial class MainForm
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tlsMain = new System.Windows.Forms.ToolStrip();
            this.tsbNewFile = new System.Windows.Forms.ToolStripButton();
            this.tsbOpenFile = new System.Windows.Forms.ToolStripButton();
            this.tsbSaveFile = new System.Windows.Forms.ToolStripButton();
            this.tsbExportProject = new System.Windows.Forms.ToolStripButton();
            this.tsbSearch = new System.Windows.Forms.ToolStripButton();
            this.tsbSearchInFiles = new System.Windows.Forms.ToolStripButton();
            this.tsExtender = new WeifenLuo.WinFormsUI.Docking.VisualStudioToolStripExtender(this.components);
            this.dockPanel = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.dockTheme = new WeifenLuo.WinFormsUI.Docking.VS2015LightTheme();
            this.LoadFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.mniFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mniFileNew = new System.Windows.Forms.ToolStripMenuItem();
            this.mniFileOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.mniFileSave = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mniFileExport = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.mniFileExit = new System.Windows.Forms.ToolStripMenuItem();
            this.mniEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.mniEditSearch = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.mniEditSearchInFiles = new System.Windows.Forms.ToolStripMenuItem();
            this.mniHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.mniHelpAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuMain = new System.Windows.Forms.MenuStrip();
            this.mniBulk = new System.Windows.Forms.ToolStripMenuItem();
            this.mniBulkTexts = new System.Windows.Forms.ToolStripMenuItem();
            this.mniBulkTextsExport = new System.Windows.Forms.ToolStripMenuItem();
            this.mniBulkTextsImport = new System.Windows.Forms.ToolStripMenuItem();
            this.mniBulkImages = new System.Windows.Forms.ToolStripMenuItem();
            this.mniBulkImagesExport = new System.Windows.Forms.ToolStripMenuItem();
            this.mniBulkImagesImport = new System.Windows.Forms.ToolStripMenuItem();
            this.FolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.tlsMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dockPanel)).BeginInit();
            this.mnuMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // tlsMain
            // 
            resources.ApplyResources(this.tlsMain, "tlsMain");
            this.tlsMain.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.tlsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbNewFile,
            this.tsbOpenFile,
            this.tsbSaveFile,
            this.tsbExportProject,
            this.toolStripSeparator1,
            this.tsbSearch,
            this.tsbSearchInFiles});
            this.tlsMain.Name = "tlsMain";
            // 
            // tsbNewFile
            // 
            this.tsbNewFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbNewFile.Image = global::TF.GUI.Icons.newfile;
            resources.ApplyResources(this.tsbNewFile, "tsbNewFile");
            this.tsbNewFile.Name = "tsbNewFile";
            this.tsbNewFile.Click += new System.EventHandler(this.FileNew_Click);
            // 
            // tsbOpenFile
            // 
            this.tsbOpenFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbOpenFile.Image = global::TF.GUI.Icons.openfolder;
            resources.ApplyResources(this.tsbOpenFile, "tsbOpenFile");
            this.tsbOpenFile.Name = "tsbOpenFile";
            this.tsbOpenFile.Click += new System.EventHandler(this.FileOpen_Click);
            // 
            // tsbSaveFile
            // 
            this.tsbSaveFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.tsbSaveFile, "tsbSaveFile");
            this.tsbSaveFile.Image = global::TF.GUI.Icons.save;
            this.tsbSaveFile.Name = "tsbSaveFile";
            this.tsbSaveFile.Click += new System.EventHandler(this.FileSave_Click);
            // 
            // tsbExportProject
            // 
            this.tsbExportProject.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.tsbExportProject, "tsbExportProject");
            this.tsbExportProject.Image = global::TF.GUI.Icons.export;
            this.tsbExportProject.Name = "tsbExportProject";
            this.tsbExportProject.Click += new System.EventHandler(this.FileExport_Click);
            // 
            // tsbSearch
            // 
            this.tsbSearch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.tsbSearch, "tsbSearch");
            this.tsbSearch.Image = global::TF.GUI.Icons.search;
            this.tsbSearch.Name = "tsbSearch";
            this.tsbSearch.Click += new System.EventHandler(this.SearchText_Click);
            // 
            // tsbSearchInFiles
            // 
            this.tsbSearchInFiles.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.tsbSearchInFiles, "tsbSearchInFiles");
            this.tsbSearchInFiles.Image = global::TF.GUI.Icons.searchfiles;
            this.tsbSearchInFiles.Name = "tsbSearchInFiles";
            this.tsbSearchInFiles.Click += new System.EventHandler(this.SearchInFiles_Click);
            // 
            // tsExtender
            // 
            this.tsExtender.DefaultRenderer = null;
            // 
            // dockPanel
            // 
            this.dockPanel.AllowEndUserDocking = false;
            this.dockPanel.AllowEndUserNestedDocking = false;
            resources.ApplyResources(this.dockPanel, "dockPanel");
            this.dockPanel.DockBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(242)))));
            this.dockPanel.Name = "dockPanel";
            this.dockPanel.Theme = null;
            // 
            // LoadFileDialog
            // 
            resources.ApplyResources(this.LoadFileDialog, "LoadFileDialog");
            // 
            // mniFile
            // 
            this.mniFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniFileNew,
            this.mniFileOpen,
            this.toolStripSeparator,
            this.mniFileSave,
            this.toolStripSeparator2,
            this.mniFileExport,
            this.toolStripSeparator5,
            this.mniFileExit});
            this.mniFile.Name = "mniFile";
            resources.ApplyResources(this.mniFile, "mniFile");
            // 
            // mniFileNew
            // 
            this.mniFileNew.Image = global::TF.GUI.Icons.newfile;
            resources.ApplyResources(this.mniFileNew, "mniFileNew");
            this.mniFileNew.Name = "mniFileNew";
            this.mniFileNew.Click += new System.EventHandler(this.FileNew_Click);
            // 
            // mniFileOpen
            // 
            this.mniFileOpen.Image = global::TF.GUI.Icons.openfolder;
            resources.ApplyResources(this.mniFileOpen, "mniFileOpen");
            this.mniFileOpen.Name = "mniFileOpen";
            this.mniFileOpen.Click += new System.EventHandler(this.FileOpen_Click);
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            resources.ApplyResources(this.toolStripSeparator, "toolStripSeparator");
            // 
            // mniFileSave
            // 
            resources.ApplyResources(this.mniFileSave, "mniFileSave");
            this.mniFileSave.Image = global::TF.GUI.Icons.save;
            this.mniFileSave.Name = "mniFileSave";
            this.mniFileSave.Click += new System.EventHandler(this.FileSave_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // mniFileExport
            // 
            resources.ApplyResources(this.mniFileExport, "mniFileExport");
            this.mniFileExport.Image = global::TF.GUI.Icons.export;
            this.mniFileExport.Name = "mniFileExport";
            this.mniFileExport.Click += new System.EventHandler(this.FileExport_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            resources.ApplyResources(this.toolStripSeparator5, "toolStripSeparator5");
            // 
            // mniFileExit
            // 
            this.mniFileExit.Name = "mniFileExit";
            resources.ApplyResources(this.mniFileExit, "mniFileExit");
            this.mniFileExit.Click += new System.EventHandler(this.mniFileExit_Click);
            // 
            // mniEdit
            // 
            this.mniEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniEditSearch,
            this.toolStripSeparator3,
            this.mniEditSearchInFiles});
            this.mniEdit.Name = "mniEdit";
            resources.ApplyResources(this.mniEdit, "mniEdit");
            // 
            // mniEditSearch
            // 
            resources.ApplyResources(this.mniEditSearch, "mniEditSearch");
            this.mniEditSearch.Image = global::TF.GUI.Icons.search;
            this.mniEditSearch.Name = "mniEditSearch";
            this.mniEditSearch.Click += new System.EventHandler(this.SearchText_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            // 
            // mniEditSearchInFiles
            // 
            resources.ApplyResources(this.mniEditSearchInFiles, "mniEditSearchInFiles");
            this.mniEditSearchInFiles.Image = global::TF.GUI.Icons.searchfiles;
            this.mniEditSearchInFiles.Name = "mniEditSearchInFiles";
            this.mniEditSearchInFiles.Click += new System.EventHandler(this.SearchInFiles_Click);
            // 
            // mniHelp
            // 
            this.mniHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniHelpAbout});
            this.mniHelp.Name = "mniHelp";
            resources.ApplyResources(this.mniHelp, "mniHelp");
            // 
            // mniHelpAbout
            // 
            this.mniHelpAbout.Name = "mniHelpAbout";
            resources.ApplyResources(this.mniHelpAbout, "mniHelpAbout");
            this.mniHelpAbout.Click += new System.EventHandler(this.HelpAbout_Click);
            // 
            // mnuMain
            // 
            this.mnuMain.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.mnuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniFile,
            this.mniEdit,
            this.mniBulk,
            this.mniHelp});
            resources.ApplyResources(this.mnuMain, "mnuMain");
            this.mnuMain.Name = "mnuMain";
            // 
            // mniBulk
            // 
            this.mniBulk.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniBulkTexts,
            this.mniBulkImages});
            this.mniBulk.Name = "mniBulk";
            resources.ApplyResources(this.mniBulk, "mniBulk");
            // 
            // mniBulkTexts
            // 
            this.mniBulkTexts.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniBulkTextsExport,
            this.mniBulkTextsImport});
            this.mniBulkTexts.Name = "mniBulkTexts";
            resources.ApplyResources(this.mniBulkTexts, "mniBulkTexts");
            // 
            // mniBulkTextsExport
            // 
            resources.ApplyResources(this.mniBulkTextsExport, "mniBulkTextsExport");
            this.mniBulkTextsExport.Name = "mniBulkTextsExport";
            this.mniBulkTextsExport.Click += new System.EventHandler(this.mniBulkTextsExport_Click);
            // 
            // mniBulkTextsImport
            // 
            resources.ApplyResources(this.mniBulkTextsImport, "mniBulkTextsImport");
            this.mniBulkTextsImport.Name = "mniBulkTextsImport";
            this.mniBulkTextsImport.Click += new System.EventHandler(this.mniBulkTextsImport_Click);
            // 
            // mniBulkImages
            // 
            this.mniBulkImages.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniBulkImagesExport,
            this.mniBulkImagesImport});
            this.mniBulkImages.Name = "mniBulkImages";
            resources.ApplyResources(this.mniBulkImages, "mniBulkImages");
            // 
            // mniBulkImagesExport
            // 
            resources.ApplyResources(this.mniBulkImagesExport, "mniBulkImagesExport");
            this.mniBulkImagesExport.Name = "mniBulkImagesExport";
            this.mniBulkImagesExport.Click += new System.EventHandler(this.mniBulkImagesExport_Click);
            // 
            // mniBulkImagesImport
            // 
            resources.ApplyResources(this.mniBulkImagesImport, "mniBulkImagesImport");
            this.mniBulkImagesImport.Name = "mniBulkImagesImport";
            this.mniBulkImagesImport.Click += new System.EventHandler(this.mniBulkImagesImport_Click);
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dockPanel);
            this.Controls.Add(this.tlsMain);
            this.Controls.Add(this.mnuMain);
            this.KeyPreview = true;
            this.MainMenuStrip = this.mnuMain;
            this.Name = "MainForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyUp);
            this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.MainForm_PreviewKeyDown);
            this.tlsMain.ResumeLayout(false);
            this.tlsMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dockPanel)).EndInit();
            this.mnuMain.ResumeLayout(false);
            this.mnuMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStripButton tsbNewFile;
        private System.Windows.Forms.ToolStripButton tsbOpenFile;
        private System.Windows.Forms.ToolStripButton tsbSaveFile;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStrip tlsMain;
        private System.Windows.Forms.ToolStripButton tsbSearch;
        private System.Windows.Forms.ToolStripButton tsbSearchInFiles;
        private WeifenLuo.WinFormsUI.Docking.VisualStudioToolStripExtender tsExtender;
        private WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel;
        private WeifenLuo.WinFormsUI.Docking.VS2015LightTheme dockTheme;
        private System.Windows.Forms.OpenFileDialog LoadFileDialog;
        private System.Windows.Forms.ToolStripButton tsbExportProject;
        private System.Windows.Forms.ToolStripMenuItem mniFile;
        private System.Windows.Forms.ToolStripMenuItem mniFileNew;
        private System.Windows.Forms.ToolStripMenuItem mniFileOpen;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem mniFileSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem mniFileExport;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem mniFileExit;
        private System.Windows.Forms.ToolStripMenuItem mniEdit;
        private System.Windows.Forms.ToolStripMenuItem mniEditSearch;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem mniEditSearchInFiles;
        private System.Windows.Forms.ToolStripMenuItem mniHelp;
        private System.Windows.Forms.ToolStripMenuItem mniHelpAbout;
        private System.Windows.Forms.MenuStrip mnuMain;
        private System.Windows.Forms.ToolStripMenuItem mniBulk;
        private System.Windows.Forms.FolderBrowserDialog FolderBrowserDialog;
        private System.Windows.Forms.ToolStripMenuItem mniBulkTexts;
        private System.Windows.Forms.ToolStripMenuItem mniBulkTextsExport;
        private System.Windows.Forms.ToolStripMenuItem mniBulkTextsImport;
        private System.Windows.Forms.ToolStripMenuItem mniBulkImages;
        private System.Windows.Forms.ToolStripMenuItem mniBulkImagesExport;
        private System.Windows.Forms.ToolStripMenuItem mniBulkImagesImport;
    }
}

