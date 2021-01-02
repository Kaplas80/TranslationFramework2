namespace TFGame.LoveEsquire.Files.SqlText
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using SQLite;
    using TF.Core.Files;
    using TF.Core.TranslationEntities;

    public class File : BinaryTextFileWithIds
    {
        public File(string gameName, string path, string changesFolder, Encoding encoding) : base(gameName, path, changesFolder, encoding)
        {
        }

        protected override IList<Subtitle> GetSubtitles()
        {
            var result = new List<Subtitle>();

            var dbConnection = new SQLiteConnection(Path, SQLiteOpenFlags.ReadOnly);

            List<SqlText> texts = dbConnection.Query<SqlText>("select TOKEN, EN from localized_texts");
            foreach (SubtitleWithId subtitle in texts.Select(sqlText => new SubtitleWithId
            {
                Id = sqlText.TOKEN,
                Text = sqlText.EN,
                Loaded = sqlText.EN,
                Translation = sqlText.EN,
            }))
            {
                subtitle.PropertyChanged += SubtitlePropertyChanged;
                result.Add(subtitle);
            }

            dbConnection.Close();

            //result.Sort();
            LoadChanges(result);

            return result;
        }

        public override void Rebuild(string outputFolder)
        {
            string outputPath = System.IO.Path.Combine(outputFolder, RelativePath);
            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(outputPath));
            System.IO.File.Copy(Path, outputPath, true);

            IList<Subtitle> subtitles = GetSubtitles();
            List<SubtitleWithId> subs = subtitles.Select(subtitle => subtitle as SubtitleWithId).ToList();

            var dbConnection = new SQLiteConnection(outputPath, SQLiteOpenFlags.ReadWrite);
            dbConnection.Execute("UPDATE localized_texts SET RU=EN;");
            dbConnection.Commit();

            dbConnection.BeginTransaction();

            foreach (SubtitleWithId subtitleWithId in subs)
            {
                if (subtitleWithId.Text != subtitleWithId.Translation)
                {
                    dbConnection.Execute($"update localized_texts set RU='{subtitleWithId.Translation.Replace("'", "''")}' where TOKEN='{subtitleWithId.Id.Replace("'", "''")}'");
                }
            }

            dbConnection.Commit();
            dbConnection.Execute("VACUUM");
            dbConnection.Close();
        }

        private class SqlText
        {
            public string TOKEN { get; set; }
            public string EN { get; set; }
        }
    }
}
