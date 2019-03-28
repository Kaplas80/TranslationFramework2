using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TF.Core.Entities;
using TF.Core.Files;

namespace TFGame.ShiningResonance.Files.Mtp
{
    class File : BinaryTextFile
    {
        public File(string path, string changesFolder, Encoding encoding) : base(path, changesFolder, encoding)
        {
        }

        public override int SubtitleCount => 1;
    }
}
