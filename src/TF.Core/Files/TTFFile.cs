using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TF.Core.Entities;

namespace TF.Core.Files
{
    public class TTFFile : TranslationFile
    {
        public TTFFile(string path, string changesFolder) : base(path, changesFolder, null)
        {
        }
    }
}
