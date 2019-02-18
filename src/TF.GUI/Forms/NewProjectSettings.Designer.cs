namespace TF.GUI.Forms
{
    partial class NewProjectSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewProjectSettings));
            this.dockPanel1 = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.txtWorkFolder = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnSearchWorkFolder = new System.Windows.Forms.Button();
            this.btnSearchInstallFolder = new System.Windows.Forms.Button();
            this.txtInstallFolder = new System.Windows.Forms.TextBox();
            this.folderBrowserDlg = new System.Windows.Forms.FolderBrowserDialog();
            this.lvGame = new System.Windows.Forms.ListView();
            this.imlGame = new System.Windows.Forms.ImageList(this.components);
            this.txtGameDescription = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // dockPanel1
            // 
            resources.ApplyResources(this.dockPanel1, "dockPanel1");
            this.dockPanel1.Name = "dockPanel1";
            // 
            // txtWorkFolder
            // 
            resources.ApplyResources(this.txtWorkFolder, "txtWorkFolder");
            this.txtWorkFolder.Name = "txtWorkFolder";
            this.txtWorkFolder.TextChanged += new System.EventHandler(this.txtWorkFolder_TextChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnSearchWorkFolder
            // 
            resources.ApplyResources(this.btnSearchWorkFolder, "btnSearchWorkFolder");
            this.btnSearchWorkFolder.Name = "btnSearchWorkFolder";
            this.btnSearchWorkFolder.UseCompatibleTextRendering = true;
            this.btnSearchWorkFolder.UseVisualStyleBackColor = true;
            this.btnSearchWorkFolder.Click += new System.EventHandler(this.btnSearchWorkFolder_Click);
            // 
            // btnSearchInstallFolder
            // 
            resources.ApplyResources(this.btnSearchInstallFolder, "btnSearchInstallFolder");
            this.btnSearchInstallFolder.Name = "btnSearchInstallFolder";
            this.btnSearchInstallFolder.UseCompatibleTextRendering = true;
            this.btnSearchInstallFolder.UseVisualStyleBackColor = true;
            this.btnSearchInstallFolder.Click += new System.EventHandler(this.btnSearchInstallFolder_Click);
            // 
            // txtInstallFolder
            // 
            resources.ApplyResources(this.txtInstallFolder, "txtInstallFolder");
            this.txtInstallFolder.Name = "txtInstallFolder";
            this.txtInstallFolder.TextChanged += new System.EventHandler(this.txtInstallFolder_TextChanged);
            // 
            // lvProjectType
            // 
            resources.ApplyResources(this.lvGame, "lvGame");
            this.lvGame.HideSelection = false;
            this.lvGame.LargeImageList = this.imlGame;
            this.lvGame.MultiSelect = false;
            this.lvGame.Name = "lvGame";
            this.lvGame.UseCompatibleStateImageBehavior = false;
            this.lvGame.SelectedIndexChanged += new System.EventHandler(this.lvGame_SelectedIndexChanged);
            // 
            // imlProjectType
            // 
            this.imlGame.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            resources.ApplyResources(this.imlGame, "imlGame");
            this.imlGame.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // txtProjectDescription
            // 
            resources.ApplyResources(this.txtGameDescription, "txtGameDescription");
            this.txtGameDescription.Name = "txtGameDescription";
            this.txtGameDescription.ReadOnly = true;
            // 
            // NewProjectSettings
            // 
            this.AcceptButton = this.btnOK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.Controls.Add(this.txtGameDescription);
            this.Controls.Add(this.lvGame);
            this.Controls.Add(this.btnSearchInstallFolder);
            this.Controls.Add(this.txtInstallFolder);
            this.Controls.Add(this.btnSearchWorkFolder);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtWorkFolder);
            this.Controls.Add(this.dockPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NewProjectSettings";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel1;
        private System.Windows.Forms.TextBox txtWorkFolder;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnSearchWorkFolder;
        private System.Windows.Forms.Button btnSearchInstallFolder;
        private System.Windows.Forms.TextBox txtInstallFolder;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDlg;
        private System.Windows.Forms.ListView lvGame;
        private System.Windows.Forms.TextBox txtGameDescription;
        private System.Windows.Forms.ImageList imlGame;
    }
}