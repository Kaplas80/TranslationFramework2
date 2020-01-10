namespace LoveEsquireVnTextBuilder
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using SQLite;

    class Program
    {
        static void Main(string[] args)
        {
            string gameFolder = @"I:\Steam\steamapps\common\Love Esquire\";
            string vnscenesFolder = string.Concat(gameFolder, @"Love Esquire_Data\StreamingAssets\Windows\vnscenes");
            string[] files = Directory.GetFiles(vnscenesFolder, "*.");

            string vntextPath = string.Concat(gameFolder, @"Love Esquire_Data\StreamingAssets\SqlText\vntext.sq");

            File.Copy(vntextPath, "vntext.sq", true);

            var dbConnection = new SQLiteConnection("vntext.sq");
            List<SQLiteConnection.ColumnInfo> tableInfo = dbConnection.GetTableInfo("localized_texts");
            if (tableInfo.All(x => x.Name != "EN"))
            {
                dbConnection.Execute("ALTER TABLE localized_texts ADD EN TEXT;");
            }
            dbConnection.Commit();
            
            foreach (string file in files)
            {
                string fileName = Path.GetFileName(file);
                File.Copy(file, fileName, true);

                RunUnityEx(fileName);
                ParseTextFiles(dbConnection);

                Directory.Delete("Unity_Assets_Files", true);
                File.Delete(fileName);
            }

            dbConnection.Commit();
            dbConnection.Close();
        }

        private static void RunUnityEx(string fileName)
        {
            using var process = new System.Diagnostics.Process
            {
                StartInfo =
                {
                    FileName = "UnityEX.exe",
                    WorkingDirectory = ".",
                    Arguments = $"export \"{fileName}\" -t txt",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                    CreateNoWindow = true
                }
            };
            
            process.Start();
            process.WaitForExit();
        }

        private static void ParseTextFiles(SQLiteConnection dbConnection)
        {
            string[] files = Directory.GetFiles("Unity_Assets_Files", "*.txt", SearchOption.AllDirectories);
            foreach (string file in files)
            {
                dbConnection.BeginTransaction();
                string[] lines = File.ReadAllLines(file);
                foreach (string line in lines)
                {
                    string[] split = line.Split('\t');

                    if (split[0] != "say")
                    {
                        continue;
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(split[5]))
                        {
                            string token = split[5];
                            string text = split[3];
                            dbConnection.Execute($"update localized_texts set EN='{text.Replace("'", "''")}' where TOKEN='{token.Replace("'", "''")}'");
                        }
                    }
                    catch (IndexOutOfRangeException)
                    {
                        // Occurs in prologue11\vel_firstv2.txt
                    }
                }
                dbConnection.Commit();
            }
        }
    }
}
