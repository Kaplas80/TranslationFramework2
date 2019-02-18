namespace TF.GUI.Forms
{
    partial class ExplorerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExplorerForm));
            this.explorerImageList = new System.Windows.Forms.ImageList(this.components);
            this.tvGameFiles = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // explorerImageList
            // 
            this.explorerImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("explorerImageList.ImageStream")));
            this.explorerImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.explorerImageList.Images.SetKeyName(0, "folder_blue.png");
            this.explorerImageList.Images.SetKeyName(1, "file_extension_chm.png");
            this.explorerImageList.Images.SetKeyName(2, "file_extension_txt.png");
            this.explorerImageList.Images.SetKeyName(3, "file_extension_bmp.png");
            this.explorerImageList.Images.SetKeyName(4, "file_extension_zip.png");
            // 
            // tvGameFiles
            // 
            resources.ApplyResources(this.tvGameFiles, "tvGameFiles");
            this.tvGameFiles.ImageList = this.explorerImageList;
            this.tvGameFiles.Name = "tvGameFiles";
            this.tvGameFiles.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvGameFiles_BeforeSelect);
            // 
            // ExplorerForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CloseButton = false;
            this.CloseButtonVisible = false;
            this.Controls.Add(this.tvGameFiles);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)(((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft) 
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight)));
            this.Name = "ExplorerForm";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ImageList explorerImageList;
        private System.Windows.Forms.TreeView tvGameFiles;
    }
}