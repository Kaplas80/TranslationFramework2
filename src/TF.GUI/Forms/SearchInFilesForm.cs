using System;
using System.Collections.Generic;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace TF.GUI.Forms
{
    public partial class SearchInFilesForm : Form
    {
        public string SearchString => txtSearchString.Text;

        protected SearchInFilesForm()
        {
            InitializeComponent();
            AutoScaleMode = AutoScaleMode.Dpi;
        }

        public SearchInFilesForm(ThemeBase theme) : this()
        {
            dockPanel1.Theme = theme;
        }

        private void UpdateAcceptButton()
        {
            btnOK.Enabled = !string.IsNullOrEmpty(txtSearchString.Text);
        }

        private void txtSearchString_TextChanged(object sender, EventArgs e)
        {
            UpdateAcceptButton();
        }
    }
}
