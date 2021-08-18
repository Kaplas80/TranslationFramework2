using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace TF.Core.Fonts
{
    public class FontCollection
    {
        private static readonly PrivateFontCollection _fonts;
        private static readonly Dictionary<string, FontFamily> _fontDictionary;

        static unsafe FontCollection()
        {
            _fonts = new PrivateFontCollection();
            _fontDictionary = new Dictionary<string, FontFamily>();

            try
            {
                fixed (byte* fontPtr = Resources.Noto_Sans_CJK)
                {
                    _fonts.AddMemoryFont((IntPtr) fontPtr, Resources.Noto_Sans_CJK.Length);
                    _fontDictionary["Noto Sans CJK JP Regular"] = _fonts.Families[0];
                }
            }
            catch (Exception)
            {
                MessageBox.Show(
                    "No se ha podido encontrar la fuente \"Noto Sans CJK JP Regular\". Puede que los textos no se muestren correctamente.");
            }
        }

        public static void Initialize()
        {

        }

        public static Font GetFont(string familyName, float emSize, FontStyle style, GraphicsUnit unit, byte gdiCharSet)
        {
            if (_fontDictionary.ContainsKey(familyName))
            {
                return new Font(_fontDictionary[familyName], emSize, style, unit, gdiCharSet);
            }

            return null;
        }
    }
}
