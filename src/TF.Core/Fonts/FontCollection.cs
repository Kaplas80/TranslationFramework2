using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace TF.Core.Fonts
{
    public class FontCollection
    {
        [DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont, IntPtr pdv, [In] ref uint pcFonts);

        private static PrivateFontCollection _fonts;
        private static Dictionary<string, int> _fontDictionary;

        static FontCollection()
        {
            _fonts = new PrivateFontCollection();
            _fontDictionary = new Dictionary<string, int>();

            var installedCollection = new InstalledFontCollection();
            if (!installedCollection.Families.Any(x => x.Name == "Noto Sans CJK JP Regular"))
            {
                try
                {
                    var fontData = Resources.Noto_Sans_CJK;
                    var fontPtr = Marshal.AllocCoTaskMem(fontData.Length);
                    Marshal.Copy(fontData, 0, fontPtr, fontData.Length);
                    uint dummy = 0;
                    _fonts.AddMemoryFont(fontPtr, fontData.Length);
                    AddFontMemResourceEx(fontPtr, (uint)fontData.Length, IntPtr.Zero, ref dummy);
                    Marshal.FreeCoTaskMem(fontPtr);

                    _fontDictionary[_fonts.Families[0].Name] = 0;
                }
                catch (Exception e)
                {
                    MessageBox.Show(
                        "No se ha podido encontrar la fuente \"Noto Sans CJK JP Regular\". Puede que los textos no se muestren correctamente.");
                }
            }
        }

        public static void Initialize()
        {

        }

        public static Font GetFont(string familyName, float emSize, FontStyle style, GraphicsUnit unit, byte gdiCharSet)
        {
            if (_fontDictionary.ContainsKey(familyName))
            {
                return new Font(_fonts.Families[_fontDictionary[familyName]], emSize, style, unit, gdiCharSet);
            }

            return new Font(familyName, emSize, style, unit, gdiCharSet);
        }
    }
}
