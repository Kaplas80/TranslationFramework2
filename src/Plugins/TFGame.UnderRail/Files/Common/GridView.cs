using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TF.Core.Entities;
using TF.Core.TranslationEntities;

namespace TFGame.UnderRail.Files.Common
{
    public class GridView : TF.Core.Views.GridView
    {
        public GridView(TranslationFile file) : base(file)
        {
        }

        public override void LoadData(IList<Subtitle> subtitles)
        {
            _subtitles = subtitles;

            SubtitleGridView.AutoGenerateColumns = false;

            var subs = new List<UnderRailSubtitle>(_subtitles.Count);
            subs.AddRange(_subtitles.Select(subtitle => subtitle as UnderRailSubtitle));

            SubtitleGridView.DataSource = subs;

            var column = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Id",
                Name = "colId",
                HeaderText = "Id",
                DefaultCellStyle = new DataGridViewCellStyle {BackColor = Color.LightGray},
                ReadOnly = true,
                SortMode = DataGridViewColumnSortMode.NotSortable,
            };
            SubtitleGridView.Columns.Add(column);

            column = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Text",
                Name = "colOriginal",
                HeaderText = "Original",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle {BackColor = Color.LightGray},
                ReadOnly = true,
                SortMode = DataGridViewColumnSortMode.NotSortable,
            };
            SubtitleGridView.Columns.Add(column);

            column = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Translation",
                Name = "colTranslation",
                HeaderText = "Traducción",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                ReadOnly = true,
                SortMode = DataGridViewColumnSortMode.NotSortable,
            };
            SubtitleGridView.Columns.Add(column);

            UpdateLabel();

            _selectedSubtitle = null;
            _selectedSubtitleIndex = -1;
        }

        protected override string GetContext(Subtitle subtitle)
        {
            return (subtitle as UnderRailSubtitle).Id;
        }
    }
}
