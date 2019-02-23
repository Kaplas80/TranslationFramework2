using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using TF.Core;
using TF.Core.Entities;
using TF.GUI.Forms;
using TF.GUI.Properties;
using WeifenLuo.WinFormsUI.Docking;

namespace TF.GUI
{
    partial class MainForm
    {
        private TranslationProject _project;
        private TranslationFile _currentFile = null;
        private string _currentSearch = string.Empty;

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

            var project = new TranslationProject(game, gameFolder, workFolder);

            var workForm = new WorkingForm(dockTheme, "Nueva traducción");
            
            workForm.DoWork += (sender, args) =>
            {
                var worker = sender as BackgroundWorker;

                try
                {
                    project.ReadTranslationFiles(worker);
                    worker.ReportProgress(-1, "FINALIZADO");
                }
                catch (Exception)
                {
                    args.Cancel = true;
                }
            };
            
            workForm.ShowDialog(this);

            if (workForm.Cancelled)
            {
                return;
            }

            _project = project;

            _explorer.LoadTree(_project.FileContainers);

            _currentFile = null;

            _project.Save();

            Text = $"Translation Framework 2.0 - {_project.Game.Name} - {_project.WorkPath}";
            tsbExportProject.Enabled = true;
            mniFileExport.Enabled = true;
            tsbSearchInFiles.Enabled = true;
            mniEditSearchInFiles.Enabled = true;
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

                TranslationProject project;
                try
                {
                    project = TranslationProject.Load(LoadFileDialog.FileName, _pluginManager);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                _project = project;
                _explorer.LoadTree(_project.FileContainers);

                _currentFile = null;

                Text = $"Translation Framework 2.0 - {_project.Game.Name} - {_project.WorkPath}";
                tsbExportProject.Enabled = true;
                mniFileExport.Enabled = true;
                tsbSearchInFiles.Enabled = true;
                mniEditSearchInFiles.Enabled = true;
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

                    try
                    {
                        _project.Export(selectedContainers, compress, worker);

                        worker.ReportProgress(-1, "FINALIZADO");
                        worker.ReportProgress(-1, string.Empty);
                        worker.ReportProgress(-1, $"Los ficheros exportados están en {_project.ExportFolder}");
                    }
                    catch (Exception e)
                    {
                        args.Cancel = true;
                    }
                };

                workForm.ShowDialog(this);
            }
        }

        private void SearchInFiles()
        {
            if (_project != null)
            {
                var form = new SearchInFilesForm(dockTheme);

                var formResult = form.ShowDialog(this);

                if (formResult == DialogResult.Cancel)
                {
                    return;
                }

                var searchString = form.SearchString;
                var workForm = new WorkingForm(dockTheme, "Buscar en ficheros", true);
                IList<Tuple<TranslationFileContainer, TranslationFile>> filesFound = null;
                workForm.DoWork += (sender, args) =>
                {
                    var worker = sender as BackgroundWorker;

                    try
                    {
                        filesFound = _project.SearchInFiles(searchString, worker);

                        worker.ReportProgress(-1, "FINALIZADO");
                    }
                    catch (Exception)
                    {
                        args.Cancel = true;
                    }
                };

                workForm.ShowDialog(this);

                if (workForm.Cancelled)
                {
                    return;
                }

                _searchResults.LoadItems(searchString, filesFound);
                if (_searchResults.VisibleState == DockState.DockBottomAutoHide)
                {
                    dockPanel.ActiveAutoHideContent = _searchResults;
                }
            }
        }

        private void SearchText()
        {
            if (_project != null && _currentFile != null && _currentFile.Type == FileType.TextFile)
            {
                var form = new SearchTextForm(dockTheme);

                var formResult = form.ShowDialog(this);

                if (formResult == DialogResult.Cancel)
                {
                    return;
                }

                _currentSearch = form.SearchString;

                var textFound = _currentFile.SearchText(_currentSearch, 0);

                if (!textFound)
                {
                    MessageBox.Show("No se han encontrado coincidencias.", "Buscar", MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
                }
            }
        }

        private void SearchText(int direction)
        {
            if (_project != null && _currentFile != null && _currentFile.Type == FileType.TextFile)
            {
                if (!string.IsNullOrEmpty(_currentSearch))
                {
                    var textFound = _currentFile.SearchText(_currentSearch, direction);

                    if (!textFound)
                    {
                        MessageBox.Show("No se han encontrado coincidencias.", "Buscar", MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
                    }
                }
            }
        }
    }
}
