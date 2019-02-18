using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TF.Core.Helpers;
using WeifenLuo.WinFormsUI.Docking;

namespace TF.GUI.Forms
{
    public partial class NewProjectSettings : Form
    {
        private IList<Core.Entities.IGame> _games;

        public string SelectedGame => lvGame.SelectedItems.Count > 0 ? lvGame.SelectedItems[0].Name : string.Empty;
        public string WorkFolder => txtWorkFolder.Text;
        public string GameFolder => txtInstallFolder.Text;

        protected NewProjectSettings()
        {
            InitializeComponent();
            AutoScaleMode = AutoScaleMode.Dpi;
        }

        public NewProjectSettings(ThemeBase theme, IList<Core.Entities.IGame> games) : this()
        {
            dockPanel1.Theme = theme;

            _games = games;

            foreach (var game in _games)
            {
                var projectId = game.Id;
                imlGame.Images.Add(projectId, game.Icon);
                lvGame.Items.Add(projectId, game.Name, projectId);
            }

            lvGame.Items[0].Selected = true;
        }

        private void btnSearchWorkFolder_Click(object sender, EventArgs e)
        {
            var description = string.Concat("Selecciona la carpeta de trabajo.", Environment.NewLine,
                "En ella se guardarán todos los ficheros necesarios para la traducción.", Environment.NewLine,
                "Asegúrate de que el disco tiene espacio suficiente.");
            SearchFolder(description, txtWorkFolder, true);
        }

        private void btnSearchInstallFolder_Click(object sender, EventArgs e)
        {
            var description = "Selecciona la carpeta dónde está el juego.";
            SearchFolder(description, txtInstallFolder, false);
        }

        private void SearchFolder(string description, Control textBox, bool showNewFolder)
        {
            folderBrowserDlg.Description = description;
            folderBrowserDlg.SelectedPath = textBox.Text;
            folderBrowserDlg.ShowNewFolderButton = showNewFolder;
            var result = folderBrowserDlg.ShowDialog(this);

            if (result == DialogResult.OK)
            {
                textBox.Text = folderBrowserDlg.SelectedPath;
            }
        }

        private void lvGame_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvGame.SelectedItems.Count > 0)
            {
                var selectedItem = lvGame.SelectedItems[0];
                var game = _games.First(x => x.Id == selectedItem.Name);
                txtGameDescription.Text = game.Description;
            }
            else
            {
                txtGameDescription.Text = string.Empty;
            }

            UpdateAcceptButton();
        }

        private void UpdateAcceptButton()
        {
            var isGameSelected = lvGame.SelectedItems.Count > 0;
            var isWorkFolderSelected = !string.IsNullOrEmpty(txtWorkFolder.Text);
            var isInstallFolderSelected = !string.IsNullOrEmpty(txtInstallFolder.Text);

            if (isGameSelected && isWorkFolderSelected && isInstallFolderSelected)
            {
                var areDifferentFolders = false;
                try
                {
                    areDifferentFolders =
                        PathHelper.GetRelativePath(txtInstallFolder.Text, txtWorkFolder.Text) != ".";
                }
                catch (ArgumentException)
                {
                    // Este error da si no tienen padre en común. En ese caso son carpetas diferentes
                    areDifferentFolders = true;
                }
                
                btnOK.Enabled = areDifferentFolders;
            }
            else
            {
                btnOK.Enabled = false;
            }
        }

        private void txtInstallFolder_TextChanged(object sender, EventArgs e)
        {
            UpdateAcceptButton();
        }

        private void txtWorkFolder_TextChanged(object sender, EventArgs e)
        {
            UpdateAcceptButton();
        }
    }
}
