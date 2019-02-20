using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TF.Core.Entities;
using WeifenLuo.WinFormsUI.Docking;

namespace TF.GUI.Forms
{
    public partial class ExportProjectForm : Form
    {
        public IList<TranslationFileContainer> SelectedContainers
        {
            get
            {
                var result = new List<TranslationFileContainer>();
                for (var i = 0; i < lbItems.Items.Count; i++)
                {
                    var isChecked = lbItems.GetItemChecked(i);
                    if (isChecked)
                    {
                        result.Add(lbItems.Items[i] as TranslationFileContainer);
                    }
                }

                return result;
            }
        }

        public bool Compression => chkCompress.Checked;

        protected ExportProjectForm()
        {
            InitializeComponent();
            AutoScaleMode = AutoScaleMode.Dpi;
        }

        public ExportProjectForm(ThemeBase theme, IList<TranslationFileContainer> containers) : this()
        {
            dockPanel1.Theme = theme;

            lbItems.Items.Clear();
            foreach (var container in containers)
            {
                lbItems.Items.Add(container, container.Files.Any(x => x.HasChanges));
            }
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            for (var i = 0; i < lbItems.Items.Count; i++)
            {
                lbItems.SetItemChecked(i, true);
            }
        }

        private void btnSelectNone_Click(object sender, EventArgs e)
        {
            for (var i = 0; i < lbItems.Items.Count; i++)
            {
                lbItems.SetItemChecked(i, false);
            }
        }

        private void btnInvertSelection_Click(object sender, EventArgs e)
        {
            for (var i = 0; i < lbItems.Items.Count; i++)
            {
                var currentState = lbItems.GetItemChecked(i);
                lbItems.SetItemChecked(i, !currentState);
            }
        }

        private void lbItems_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            for (var i = 0; i < lbItems.Items.Count; i++)
            {
                bool currentState;
                if (i == e.Index)
                {
                    currentState = e.NewValue == CheckState.Checked;
                }
                else
                {
                    currentState = lbItems.GetItemChecked(i);
                }

                if (currentState)
                {
                    btnOK.Enabled = true;
                    return;
                }
            }

            btnOK.Enabled = false;
        }

        private void btnOnlyModified_Click(object sender, EventArgs e)
        {
            for (var i = 0; i < lbItems.Items.Count; i++)
            {
                var isChecked = ((TranslationFileContainer) lbItems.Items[i]).Files.Any(x => x.HasChanges);
                lbItems.SetItemChecked(i, isChecked);
            }
        }
    }
}
