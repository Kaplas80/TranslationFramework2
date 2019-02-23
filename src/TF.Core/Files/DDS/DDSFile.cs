using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DirectXTexNet;
using TF.Core.Entities;
using WeifenLuo.WinFormsUI.Docking;

namespace TF.Core.Files.DDS
{
    public class DDSFile : TranslationFile
    {
        private DDSView _view;

        public DDSFile(string path, string changesFolder) : base(path, changesFolder, null)
        {
            this.Type = FileType.ImageFile;
        }

        public override void Open(DockPanel panel, ThemeBase theme)
        {
            _view = new DDSView(theme);
            _view.NewImageLoaded += FormOnNewImageLoaded;

            try
            {
                UpdateFormImage();
                _view.Show(panel, DockState.Document);
            }
            catch (Exception e)
            {

            }
        }

        private void FormOnNewImageLoaded(string filename)
        {
            File.Copy(filename, ChangesFile, true);

            UpdateFormImage();
        }

        private void UpdateFormImage()
        {
            var dds = GetImage();
            _view.LoadImage(dds);
        }

        private ScratchImage GetImage()
        {
            var source = HasChanges ? ChangesFile : Path;
            var dds = DirectXTexNet.TexHelper.Instance.LoadFromDDSFile(source, DDS_FLAGS.NONE);

            return dds;
        }
    }
}
