using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using TF.Core;
using TF.Core.Entities;
using TF.GUI.Forms;
using TF.GUI.Properties;

namespace TF.GUI
{
    partial class MainForm
    {
        private TranslationProject _project;
        private TranslationFile _currentFile = null;

        private void SaveSettings()
        {
            SaveDockSettings();
            Settings.Default.Save();
        }

        private void CreateNewTranslation()
        {
            var infos = _pluginManager.GetAllGames();
            var form = new NewProjectSettings(dockTheme, infos);
            var formResult = form.ShowDialog(this);

            if (formResult == DialogResult.Cancel)
            {
                return;
            }

            if (!CloseAllDocuments())
            {
                return;
            }

            var game = _pluginManager.GetGame(form.SelectedGame);
            var workFolder = form.WorkFolder;
            var gameFolder = form.GameFolder;

            if (Directory.Exists(workFolder))
            {
                var files = Directory.GetFiles(workFolder);
                var directories = Directory.GetDirectories(workFolder);

                if (files.Length + directories.Length > 0)
                {
#if DEBUG
                    Directory.Delete(workFolder, true);
                    Directory.CreateDirectory(workFolder);
#else
                    MessageBox.Show($"La carpeta {workFolder} no está vacía. Debes elegir una carpeta vacía.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
#endif
                }
            }
            else
            {
                Directory.CreateDirectory(workFolder);
            }

            _project = new TranslationProject(game, gameFolder, workFolder);

            var workForm = new WorkingForm(dockTheme, "Nueva traducción");
            
            workForm.DoWork += (sender, args) =>
            {
                var worker = sender as BackgroundWorker;

                _project.ReadTranslationFiles(worker);

                worker.ReportProgress(-1, "FINALIZADO");
            };
            
            workForm.ShowDialog(this);
            
            _explorer.LoadTree(_project.FileContainers);

            _currentFile = null;

            _project.Save();

            Text = $"Translation Framework 2.0 - {_project.Game.Name} - {_project.WorkPath}";
            tsbExportProject.Enabled = true;
            mniFileExport.Enabled = true;
        }

        private void LoadTranslation()
        {
            var dialogResult = LoadFileDialog.ShowDialog(this);

            if (dialogResult == DialogResult.OK)
            {
                if (!CloseAllDocuments())
                {
                    return;
                }

                try
                {
                    _project = TranslationProject.Load(LoadFileDialog.FileName, _pluginManager);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                _explorer.LoadTree(_project.FileContainers);

                _currentFile = null;

                Text = $"Translation Framework 2.0 - {_project.Game.Name} - {_project.WorkPath}";
                tsbExportProject.Enabled = true;
                mniFileExport.Enabled = true;
            }
        }

        private void SaveChanges()
        {
            _currentFile?.SaveChanges();
        }

        private void ExportProject()
        {
            if (_project != null)
            {
                var form = new ExportProjectForm(dockTheme, _project.FileContainers);

                var formResult = form.ShowDialog(this);

                if (formResult == DialogResult.Cancel)
                {
                    return;
                }

                var selectedContainers = form.SelectedContainers;
                var compress = form.Compression;

                var workForm = new WorkingForm(dockTheme, "Exportar traducción");

                workForm.DoWork += (sender, args) =>
                {
                    var worker = sender as BackgroundWorker;

                    _project.Export(selectedContainers, compress, worker);

                    worker.ReportProgress(-1, "FINALIZADO");
                    worker.ReportProgress(-1, string.Empty);
                    worker.ReportProgress(-1, $"Los ficheros exportados están en {_project.ExportFolder}");
                };

                workForm.ShowDialog(this);
            }
        }
    }
}
