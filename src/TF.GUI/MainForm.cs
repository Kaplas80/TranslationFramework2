using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using TF.Core;

namespace TF.GUI
{
    public partial class MainForm : Form
    {
        private PluginManager _pluginManager;

        public MainForm()
        {
            InitializeComponent();
            AutoScaleMode = AutoScaleMode.Dpi;

            ConfigureDock();

            _pluginManager = new PluginManager();
            _pluginManager.LoadPlugins(Path.Combine(Application.StartupPath, "plugins"));

            Core.Fonts.FontCollection.Initialize();
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            if (_pluginManager.GetAllGames().Count == 0)
            {
                MessageBox.Show("No se ha podido encontrar ningún plugin válido.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!CloseAllDocuments())
            {
                e.Cancel = true;
                return;
            }
            SaveSettings();
        }

        private void mniFileExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FileNew_Click(object sender, EventArgs e)
        {
            CreateNewTranslation();
        }

        private void FileOpen_Click(object sender, EventArgs e)
        {
            LoadTranslation();
        }

        private void FileSave_Click(object sender, EventArgs e)
        {
            SaveChanges();
        }

        private void HelpAbout_Click(object sender, EventArgs e)
        {

        }

        private void FileExport_Click(object sender, EventArgs e)
        {
            ExportProject();
        }

        private void SearchInFiles_Click(object sender, EventArgs e)
        {
            SearchInFiles();
        }

        private void SearchText_Click(object sender, EventArgs e)
        {
            SearchText();
        }

        private void MainForm_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
            {
                if (e.Shift)
                {
                    SearchText(-1);
                }
                else
                {
                    SearchText(1);
                }
            }

            if (e.KeyCode == Keys.F4)
            {
                _explorer.SelectNext();
            }
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
            {
                if (e.Shift)
                {
                    SearchText(-1);
                }
                else
                {
                    SearchText(1);
                }
            }

            if (e.KeyCode == Keys.F4)
            {
                _explorer.SelectNext();
            }
        }
    }
}
