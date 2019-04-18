using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFGame.TheMissing.Files.Txt
{
    public class Subtitle : TF.Core.TranslationEntities.Subtitle
    {
        public int MsgId { get; set; }

        public Subtitle(TF.Core.TranslationEntities.Subtitle s, int id)
        {
            Offset = s.Offset;
            Text = s.Text;
            Translation = s.Translation;
            Loaded = s.Loaded;
            MsgId = id;
        }
    }
}
