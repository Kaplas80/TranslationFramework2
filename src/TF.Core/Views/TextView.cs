using System.Drawing;
using ScintillaNET;
using TF.Core.TranslationEntities;
using WeifenLuo.WinFormsUI.Docking;

namespace TF.Core.Views
{
    public partial class TextView : DockContent
    {
        private PlainText _text;

        protected TextView()
        {
            InitializeComponent();
            InitScintilla(scintillaTranslation);
        }

        public TextView(ThemeBase theme) : this()
        {
            dockPanel1.Theme = theme;
            dockPanel1.DocumentStyle = DocumentStyle.DockingSdi;
        }

        private static void InitScintilla(Scintilla scintilla)
        {
            scintilla.StyleResetDefault();
            scintilla.Styles[Style.Default].Font = "Consolas";
            scintilla.Styles[Style.Default].Size = 11;
            scintilla.StyleClearAll();

            scintilla.Styles[Style.Cpp.String].ForeColor = Color.Blue;
            scintilla.Lexer = Lexer.Cpp;

            scintilla.Margins[1].Width = 50;
            scintilla.Margins[1].Type = MarginType.Number;
            scintilla.Margins[1].Mask = 0;
        }

        public void LoadData(PlainText text)
        {
            _text = text;
            scintillaTranslation.Text = _text.Translation;
        }

        private void scintillaTranslation_TextChanged(object sender, System.EventArgs e)
        {
            _text.Translation = scintillaTranslation.Text;
        }

        public bool SearchText(string searchString, int direction)
        {
            var searchStart = scintillaTranslation.CurrentPosition;

            if (direction == 0)
            {
                searchStart = 0;
            }

            if (direction >= 0)
            {
                scintillaTranslation.TargetStart = searchStart;
                scintillaTranslation.TargetEnd = scintillaTranslation.TextLength;
                scintillaTranslation.SearchFlags = SearchFlags.None;

                if (scintillaTranslation.SearchInTarget(searchString) == -1)
                {
                    return false;
                }
            }
            else
            {
                scintillaTranslation.TargetStart = searchStart - searchString.Length;
                scintillaTranslation.TargetEnd = 0;
                scintillaTranslation.SearchFlags = SearchFlags.None;

                if (scintillaTranslation.SearchInTarget(searchString) == -1)
                {
                    return false;
                }
            }

            scintillaTranslation.SetSelection(scintillaTranslation.TargetEnd, scintillaTranslation.TargetStart);
            scintillaTranslation.ScrollCaret();

            return true;
        }
    }
}
