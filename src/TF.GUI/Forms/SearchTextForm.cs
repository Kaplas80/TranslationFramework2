using System;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace TF.GUI.Forms
{
    public partial class SearchTextForm : Form
    {
        public string SearchString => txtSearchString.Text;

        protected SearchTextForm()
        {
            InitializeComponent();
            AutoScaleMode = AutoScaleMode.Dpi;
        }

        public SearchTextForm(ThemeBase theme) : this()
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
