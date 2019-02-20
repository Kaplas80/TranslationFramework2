using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows.Forms;
using TF.Core.Entities;
using TF.GUI.Forms;
using TF.GUI.Properties;
using WeifenLuo.WinFormsUI.Docking;

namespace TF.GUI
{
    partial class MainForm
    {
        private readonly ToolStripRenderer _toolStripProfessionalRenderer = new ToolStripProfessionalRenderer();
        
        private ExplorerForm _explorer;

        private void ConfigureDock()
        {
            _explorer = new ExplorerForm();
            _explorer.FileChanged += ExplorerOnFileChanged;
            _explorer.RestoreItem += ExplorerOnRestoreItem;
            tsExtender.DefaultRenderer = _toolStripProfessionalRenderer;
            
            dockPanel.Theme = dockTheme;
            dockPanel.DocumentStyle = DocumentStyle.DockingSdi;
            EnableVsRenderer(VisualStudioToolStripExtender.VsVersion.Vs2015, dockTheme);

            if (!string.IsNullOrEmpty(Settings.Default.WindowLayout))
            {
                using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(Settings.Default.WindowLayout)))
                {
                    dockPanel.LoadFromXml(ms, GetContentFromPersistString);
                }

                if (_explorer.DockPanel == null)
                {
                    _explorer.Show(dockPanel, DockState.DockLeft);
                }
            }
            else
            {
                _explorer.Show(dockPanel, DockState.DockLeft);
            }
        }

        private void ExplorerOnRestoreItem(object selectedNode)
        {
            if (selectedNode is TranslationFileContainer container)
            {
                var result =
                    MessageBox.Show(
                        "Esto restaurará los ficheros originales de este contenedor. Esta operación no se puede deshacer.\n¿Quieres continuar?",
                        "Restaurar ficheros", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    container.Restore();
                    RefreshView();
                }

                return;
            }

            if (selectedNode is TranslationFile file)
            {
                var result =
                    MessageBox.Show(
                        "Esto restaurará el fichero original. Esta operación no se puede deshacer.\n¿Quieres continuar?",
                        "Restaurar ficheros", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    file.Restore();
                    RefreshView();
                }

                return;
            }
        }

        private bool ExplorerOnFileChanged(TranslationFile selectedFile)
        {
            if (CloseAllDocuments())
            {
                _currentFile = selectedFile;

                if (_currentFile != null)
                {
                    _currentFile.Open(dockPanel, dockTheme);
                    _currentFile.FileChanged += SelectedFileChanged;
                }

                return false;
            }

            return true;
        }

        private void RefreshView()
        {
            if (CloseAllDocuments())
            {
                if (_currentFile != null)
                {
                    _currentFile.Open(dockPanel, dockTheme);
                    _currentFile.FileChanged += SelectedFileChanged;
                }
            }
        }

        private void SelectedFileChanged()
        {
            mniFileSave.Enabled = _currentFile.NeedSaving;
            tsbSaveFile.Enabled = _currentFile.NeedSaving;
        }

        private bool CloseAllDocuments()
        {
            if (_currentFile != null)
            {
                if (_currentFile.NeedSaving)
                {
                    var result = MessageBox.Show("Hay cambios pendientes en el fichero.\n¿Quieres guardarlos?",
                        "Guardar cambios", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                    if (result == DialogResult.Cancel)
                    {
                        return false;
                    }

                    if (result == DialogResult.Yes)
                    {
                        _currentFile.SaveChanges();
                    }
                }

                _currentFile.FileChanged -= SelectedFileChanged;
            }

            var documents = dockPanel.DocumentsToArray();
            foreach (var document in documents)
            {
                document.DockHandler.DockPanel = null;
            }

            return true;
        }

        private void EnableVsRenderer(VisualStudioToolStripExtender.VsVersion version, ThemeBase theme)
        {
            tsExtender.SetStyle(mnuMain, version, theme);
            tsExtender.SetStyle(tlsMain, version, theme);
        }

        private IDockContent GetContentFromPersistString(string persistString)
        {
            if (persistString == typeof(ExplorerForm).ToString())
            {
                return _explorer;
            }

            return null;
        }

        private void SaveDockSettings()
        {
            using (var ms = new MemoryStream())
            {
                dockPanel.SaveAsXml(ms, Encoding.UTF8);
                ms.Seek(0, SeekOrigin.Begin);

                var buff = ms.GetBuffer();
                Settings.Default.WindowLayout = Encoding.UTF8.GetString(buff);
            }
        }
    }
}
