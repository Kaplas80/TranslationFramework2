namespace TF.GUI.Forms
{
    partial class SearchResultsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SearchResultsForm));
            this.lbSearchResult = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // lbSearchResult
            // 
            resources.ApplyResources(this.lbSearchResult, "lbSearchResult");
            this.lbSearchResult.FormattingEnabled = true;
            this.lbSearchResult.Name = "lbSearchResult";
            this.lbSearchResult.SelectedIndexChanged += new System.EventHandler(this.lbSearchResult_SelectedIndexChanged);
            // 
            // SearchResultsForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CloseButton = false;
            this.CloseButtonVisible = false;
            this.Controls.Add(this.lbSearchResult);
            this.DockAreas = WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom;
            this.Name = "SearchResultsForm";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ListBox lbSearchResult;
    }
}