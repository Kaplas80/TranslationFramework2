using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace YakuzaCommon.Files.Exe
{
    public partial class PatchView : DockContent
    {
        protected PatchView()
        {
            InitializeComponent();
        }

        public PatchView(ThemeBase theme) : this()
        {
            dockPanel1.Theme = theme;

            dockPanel1.DocumentStyle = DocumentStyle.DockingSdi;
        }

        internal void LoadPatches(IList<ExePatch> data)
        {
            var x = 6;
            var y = 6;

            for (var i = 0; i < data.Count; i++)
            {
                var patch = data[i];

                var chk = new CheckBox
                {
                    Name = $"chk_{i}",
                    Text = patch.Name,
                    Checked = patch.Enabled,
                    Location = new Point(x, y),
                    AutoSize = true,
                    Tag = patch
                };
                chk.CheckedChanged += OnCheckedChanged;
                y += chk.Height + 6;

                toolTip1.SetToolTip(chk, patch.Description);

                dockPanel1.Controls.Add(chk);
            }
        }

        private void OnCheckedChanged(object sender, EventArgs e)
        {
            var chk = sender as CheckBox;
            var patch = chk.Tag as ExePatch;
            patch.Enabled = chk.Checked;
        }
    }
}
