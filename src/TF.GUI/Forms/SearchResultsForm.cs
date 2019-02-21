using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using TF.Core.Entities;
using WeifenLuo.WinFormsUI.Docking;

namespace TF.GUI.Forms
{
    public partial class SearchResultsForm : DockContent
    {
        private class TupleView
        {
            public TranslationFileContainer Container;
            public TranslationFile File;

            public TupleView(Tuple<TranslationFileContainer, TranslationFile> tuple)
            {
                Container = tuple.Item1;
                File = tuple.Item2;
            }

            public override string ToString()
            {
                return $"{Path.Combine(Container.Path, File.RelativePath)}";
            }
        }

        public delegate bool FileChangedHandler(TranslationFile selectedFile);

        public event FileChangedHandler FileChanged;

        public SearchResultsForm()
        {
            InitializeComponent();
            AutoScaleMode = AutoScaleMode.Dpi;
        }

        protected virtual bool OnFileChanged(TranslationFile selectedFile)
        {
            if (FileChanged != null)
            {
                var cancel = FileChanged.Invoke(selectedFile);
                return cancel;
            }

            return false;
        }

        public void LoadItems(string searchString, IList<Tuple<TranslationFileContainer, TranslationFile>> filesFound)
        {
            lbSearchResult.Items.Clear();
            if (filesFound.Count > 0)
            {
                lbSearchResult.Items.Add($"El texto \"{searchString}\" aparece en {filesFound.Count} ficheros del proyecto");
                foreach (var tuple in filesFound)
                {
                    lbSearchResult.Items.Add(new TupleView(tuple));
                }
            }
            else
            {
                lbSearchResult.Items.Add($"El texto \"{searchString}\" no aparece en ningún fichero del proyecto");
            }
        }

        private void lbSearchResult_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedItem = lbSearchResult.SelectedItem;
            if (selectedItem is TupleView tuple)
            {
                OnFileChanged(tuple.File);
            }
        }
    }
}
