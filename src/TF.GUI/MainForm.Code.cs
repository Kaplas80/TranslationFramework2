using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using TF.Core;
using TF.Core.Entities;
using TF.Core.Exceptions;
using TF.Core.Helpers;
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
                    PathHelper.DeleteDirectory(workFolder);
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
                catch (UserCancelException)
                {
                    args.Cancel = true;
                    worker.ReportProgress(-1, "Eliminando ficheros...");
                    PathHelper.DeleteDirectory(workFolder);
                    worker.ReportProgress(-1, "Terminado");
                }
#if !DEBUG
                catch (Exception e)
                {
                    worker.ReportProgress(0, $"ERROR: {e.Message}\n{e.StackTrace}");
                }
#endif
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

            // Po
            mniBulkTextsExportPo.Enabled = true;
            mniBulkTextsImportPo.Enabled = true;

            // Excel
            mniBulkTextsExportExcel.Enabled = true;
            mniBulkTextsImportExcel.Enabled = true;

            // Images
            mniBulkImagesExport.Enabled = true;
            mniBulkImagesImport.Enabled = true;
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

                var workForm = new WorkingForm(dockTheme, "Cargar traducción", true);

                TranslationProject project = null;

                workForm.DoWork += (sender, args) =>
                {
                    var worker = sender as BackgroundWorker;

                    try
                    {
                        project = TranslationProject.Load(LoadFileDialog.FileName, _pluginManager, worker);
                    }
                    catch (UserCancelException)
                    {
                        args.Cancel = true;
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                };

                workForm.ShowDialog(this);

                if (workForm.Cancelled)
                {
                    return;
                }

                _project = project;
                _currentFile = null;

                _explorer.LoadTree(_project.FileContainers);

                _project.Save();

                Text = $"Translation Framework 2.0 - {_project.Game.Name} - {_project.WorkPath}";
                tsbExportProject.Enabled = true;
                mniFileExport.Enabled = true;
                tsbSearchInFiles.Enabled = true;
                mniEditSearchInFiles.Enabled = true;

                // Po
                mniBulkTextsExportPo.Enabled = true;
                mniBulkTextsImportPo.Enabled = true;

                // Excel
                mniBulkTextsExportExcel.Enabled = true;
                mniBulkTextsImportExcel.Enabled = true;

                // Images
                mniBulkImagesExport.Enabled = true;
                mniBulkImagesImport.Enabled = true;
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
                if (_currentFile != null)
                {
                    if (_currentFile.NeedSaving)
                    {
                        var result = MessageBox.Show(
                            "Es necesario guardar los cambios antes de continuar.\n¿Quieres guardarlos?",
                            "Guardar cambios", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (result == DialogResult.No)
                        {
                            return;
                        }

                        if (result == DialogResult.Yes)
                        {
                            _currentFile.SaveChanges();
                        }
                    }
                }

                var form = new ExportProjectForm(dockTheme, _project.FileContainers);

                var formResult = form.ShowDialog(this);

                if (formResult == DialogResult.Cancel)
                {
                    return;
                }

                var selectedContainers = form.SelectedContainers;
                var options = form.Options;

                var workForm = new WorkingForm(dockTheme, "Exportar traducción");

                workForm.DoWork += (sender, args) =>
                {
                    var worker = sender as BackgroundWorker;

                    try
                    {
                        _project.Export(selectedContainers, options, worker);

                        worker.ReportProgress(-1, "FINALIZADO");
                        worker.ReportProgress(-1, string.Empty);
                        worker.ReportProgress(-1, $"Los ficheros exportados están en {_project.ExportFolder}");
                    }
                    catch (UserCancelException)
                    {
                        args.Cancel = true;
                    }
#if !DEBUG
                    catch (Exception e)
                    {
                        worker.ReportProgress(0, $"ERROR: {e.Message}");
                    }
#endif
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
                    catch (UserCancelException)
                    {
                        args.Cancel = true;
                    }
                    catch (Exception e)
                    {
                        worker.ReportProgress(0, $"ERROR: {e.Message}");
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

        private void ExportTextsPo()
        {
            if (_project != null)
            {
                if (_currentFile != null)
                {
                    if (_currentFile.NeedSaving)
                    {
                        var result = MessageBox.Show(
                            "Es necesario guardar los cambios antes de continuar.\n¿Quieres guardarlos?",
                            "Guardar cambios", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (result == DialogResult.No)
                        {
                            return;
                        }

                        if (result == DialogResult.Yes)
                        {
                            _currentFile.SaveChanges();
                        }
                    }
                }

                FolderBrowserDialog.Description = "Selecciona la carpeta en la que se guardarán los ficheros Po";
                FolderBrowserDialog.ShowNewFolderButton = true;

                var formResult = FolderBrowserDialog.ShowDialog(this);

                if (formResult == DialogResult.Cancel)
                {
                    return;
                }

                var workForm = new WorkingForm(dockTheme, "Exportar a Po");

                workForm.DoWork += (sender, args) =>
                {
                    var worker = sender as BackgroundWorker;

#if !DEBUG
                    try
                    {
#endif
                        _project.ExportPo(FolderBrowserDialog.SelectedPath, worker);

                        worker.ReportProgress(-1, "FINALIZADO");
                        worker.ReportProgress(-1, string.Empty);
                        worker.ReportProgress(-1, $"Los ficheros exportados están en {FolderBrowserDialog.SelectedPath}");
#if !DEBUG
                    }
                    catch (UserCancelException e)
                    {
                        args.Cancel = true;
                    }

                    catch (Exception e)
                    {
                        worker.ReportProgress(0, $"ERROR: {e.Message}");
                    }
#endif
                };

                workForm.ShowDialog(this);
            }
        }

        private void ExportTextsExcel()
        {
            if (_project == null) return;

            if (_currentFile != null)
            {
                if (_currentFile.NeedSaving)
                {
                    var result = MessageBox.Show(
                        "Es necesario guardar los cambios antes de continuar.\n¿Quieres guardarlos?",
                        "Guardar cambios", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result != DialogResult.No)
                    {
                        if (result == DialogResult.Yes)
                        {
                            _currentFile.SaveChanges();
                        }
                    }
                    else
                        return;
                }
            }

            FolderBrowserDialog.Description = "Selecciona la carpeta en la que se guardarán los ficheros Excel";
            FolderBrowserDialog.ShowNewFolderButton = true;

            var formResult = FolderBrowserDialog.ShowDialog(this);

            if (formResult == DialogResult.Cancel)
            {
                return;
            }

            var workForm = new WorkingForm(dockTheme, "Exportar a Excel");

            workForm.DoWork += (sender, args) =>
            {
                var worker = sender as BackgroundWorker;

#if !DEBUG
                try
                {
#endif
                    _project.ExportExcel(FolderBrowserDialog.SelectedPath, worker);

                    worker.ReportProgress(-1, "FINALIZADO");
                    worker.ReportProgress(-1, string.Empty);
                    worker.ReportProgress(-1, $"Los ficheros exportados están en {FolderBrowserDialog.SelectedPath}");
#if !DEBUG
                }
                catch (UserCancelException e)
                {
                    args.Cancel = true;
                }

                catch (Exception e)
                {
                    worker.ReportProgress(0, $"ERROR: {e.Message}");
                }
#endif
            };

            workForm.ShowDialog(this);
        }

        private void ExportImages()
        {
            if (_project != null)
            {
                if (_currentFile != null)
                {
                    if (_currentFile.NeedSaving)
                    {
                        var result = MessageBox.Show(
                            "Es necesario guardar los cambios antes de continuar.\n¿Quieres guardarlos?",
                            "Guardar cambios", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (result == DialogResult.No)
                        {
                            return;
                        }

                        if (result == DialogResult.Yes)
                        {
                            _currentFile.SaveChanges();
                        }
                    }
                }

                FolderBrowserDialog.Description = "Selecciona la carpeta en la que se guardarán las imágenes";
                FolderBrowserDialog.ShowNewFolderButton = true;

                var formResult = FolderBrowserDialog.ShowDialog(this);

                if (formResult == DialogResult.Cancel)
                {
                    return;
                }

                var workForm = new WorkingForm(dockTheme, "Exportar Imágenes");

                workForm.DoWork += (sender, args) =>
                {
                    var worker = sender as BackgroundWorker;

#if !DEBUG
                    try
                    {
#endif
                        _project.ExportImages(FolderBrowserDialog.SelectedPath, worker);

                        worker.ReportProgress(-1, "FINALIZADO");
                        worker.ReportProgress(-1, string.Empty);
                        worker.ReportProgress(-1, $"Los ficheros exportados están en {FolderBrowserDialog.SelectedPath}");
#if !DEBUG
                    }
                    catch (UserCancelException e)
                    {
                        args.Cancel = true;
                    }

                    catch (Exception e)
                    {
                        worker.ReportProgress(0, $"ERROR: {e.Message}");
                    }
#endif
                };

                workForm.ShowDialog(this);
            }
        }

        private void ImportTexts()
        {
            if (_project != null)
            {
                if (_currentFile != null)
                {
                    if (_currentFile.NeedSaving)
                    {
                        var result = MessageBox.Show(
                            "Es necesario guardar los cambios antes de continuar.\n¿Quieres guardarlos?",
                            "Guardar cambios", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (result == DialogResult.No)
                        {
                            return;
                        }

                        if (result == DialogResult.Yes)
                        {
                            _currentFile.SaveChanges();
                        }
                    }
                }

                FolderBrowserDialog.Description = "Selecciona la carpeta raiz con los ficheros Po";
                FolderBrowserDialog.ShowNewFolderButton = false;

                var formResult = FolderBrowserDialog.ShowDialog(this);

                if (formResult == DialogResult.Cancel)
                {
                    return;
                }

                var openFile = _currentFile;
                ExplorerOnFileChanged(null);

                var workForm = new WorkingForm(dockTheme, "Importar Po");

                workForm.DoWork += (sender, args) =>
                {
                    var worker = sender as BackgroundWorker;

                    try
                    {
                        _project.ImportPo(FolderBrowserDialog.SelectedPath, worker);

                        worker.ReportProgress(-1, "FINALIZADO");
                        worker.ReportProgress(-1, string.Empty);
                    }
                    catch (UserCancelException)
                    {
                        args.Cancel = true;
                    }
#if !DEBUG
                    catch (Exception e)
                    {
                        worker.ReportProgress(0, $"ERROR: {e.Message}");
                    }
#endif
                };

                workForm.ShowDialog(this);

                ExplorerOnFileChanged(openFile);
            }
        }

        private void ImportTextsExcel()
        {
            if (_project != null)
            {
                if (_currentFile != null)
                {
                    if (_currentFile.NeedSaving)
                    {
                        var result = MessageBox.Show(
                            "Es necesario guardar los cambios antes de continuar.\n¿Quieres guardarlos?",
                            "Guardar cambios", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (result == DialogResult.No)
                        {
                            return;
                        }

                        if (result == DialogResult.Yes)
                        {
                            _currentFile.SaveChanges();
                        }
                    }
                }

                FolderBrowserDialog.Description = "Selecciona la carpeta raiz con los ficheros Excel";
                FolderBrowserDialog.ShowNewFolderButton = false;

                var formResult = FolderBrowserDialog.ShowDialog(this);

                if (formResult == DialogResult.Cancel)
                {
                    return;
                }

                var openFile = _currentFile;
                ExplorerOnFileChanged(null);

                var workForm = new WorkingForm(dockTheme, "Importar Excel");

                workForm.DoWork += (sender, args) =>
                {
                    var worker = sender as BackgroundWorker;

                    try
                    {
                        _project.ImportExcel(FolderBrowserDialog.SelectedPath, worker);

                        worker.ReportProgress(-1, "FINALIZADO");
                        worker.ReportProgress(-1, string.Empty);
                    }
                    catch (UserCancelException e)
                    {
                        args.Cancel = true;
                    }
#if !DEBUG
                    catch (Exception e)
                    {
                        worker.ReportProgress(0, $"ERROR: {e.Message}");
                    }
#endif
                };

                workForm.ShowDialog(this);

                ExplorerOnFileChanged(openFile);
            }
        }

        private void ImportImages()
        {
            if (_project != null)
            {
                if (_currentFile != null)
                {
                    if (_currentFile.NeedSaving)
                    {
                        var result = MessageBox.Show(
                            "Es necesario guardar los cambios antes de continuar.\n¿Quieres guardarlos?",
                            "Guardar cambios", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (result == DialogResult.No)
                        {
                            return;
                        }

                        if (result == DialogResult.Yes)
                        {
                            _currentFile.SaveChanges();
                        }
                    }
                }

                FolderBrowserDialog.Description = "Selecciona la carpeta raiz con las imágenes";
                FolderBrowserDialog.ShowNewFolderButton = false;

                var formResult = FolderBrowserDialog.ShowDialog(this);

                if (formResult == DialogResult.Cancel)
                {
                    return;
                }

                var openFile = _currentFile;
                ExplorerOnFileChanged(null);
                
                var workForm = new WorkingForm(dockTheme, "Importar Imágenes");

                workForm.DoWork += (sender, args) =>
                {
                    var worker = sender as BackgroundWorker;

                    try
                    {
                        _project.ImportImages(FolderBrowserDialog.SelectedPath, worker);

                        worker.ReportProgress(-1, "FINALIZADO");
                        worker.ReportProgress(-1, string.Empty);
                    }
                    catch (UserCancelException)
                    {
                        args.Cancel = true;
                    }
#if !DEBUG
                    catch (Exception e)
                    {
                        worker.ReportProgress(0, $"ERROR: {e.Message}");
                    }
#endif
                };

                workForm.ShowDialog(this);

                ExplorerOnFileChanged(openFile);
            }
        }
    }
}
