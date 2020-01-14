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
            string gameFolder = @"I:\Steam\steamapps\common\Love Esquire\Love Esquire_Data\";

            string vntextPath = string.Concat(gameFolder, @"StreamingAssets\SqlText\vntext.sq");
            File.Copy(vntextPath, "vntext.sq", true);

            string vnscenesFolder = string.Concat(gameFolder, @"StreamingAssets\Windows\vnscenes");
            string[] vnfiles = Directory.GetFiles(vnscenesFolder, "*.");

            string resources = string.Concat(gameFolder, "resources.assets");
            string sharedAssets0 = string.Concat(gameFolder, "sharedassets0.assets");

            var dbConnection = new SQLiteConnection("vntext.sq");
            List<SQLiteConnection.ColumnInfo> tableInfo = dbConnection.GetTableInfo("localized_texts");
            if (tableInfo.All(x => x.Name != "EN"))
            {
                dbConnection.Execute("ALTER TABLE localized_texts ADD EN TEXT;");
            }
            dbConnection.Commit();

            string fileName;
            foreach (string file in vnfiles)
            {
                fileName = Path.GetFileName(file);
                File.Copy(file, fileName, true);

                RunUnityEx(fileName);
                ParseVnTextFiles(dbConnection);

                Directory.Delete("Unity_Assets_Files", true);
                File.Delete(fileName);
            }

            fileName = Path.GetFileName(resources);
            File.Copy(resources, fileName, true);
            RunUnityEx(fileName);
            ParseResourcesTextFiles(dbConnection);
            Directory.Delete("Unity_Assets_Files", true);
            File.Delete(fileName);

            fileName = Path.GetFileName(sharedAssets0);
            File.Copy(sharedAssets0, fileName, true);
            RunUnityEx(fileName);
            ParseSharedAssets0TextFiles(dbConnection);
            Directory.Delete("Unity_Assets_Files", true);
            File.Delete(fileName);

            dbConnection.Execute("UPDATE localized_texts SET EN=DE WHERE EN IS NULL");
            dbConnection.Execute("VACUUM");
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

        private static void ParseVnTextFiles(SQLiteConnection dbConnection)
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

        private static void ParseResourcesTextFiles(SQLiteConnection dbConnection)
        {
            string[] files = Directory.GetFiles("Unity_Assets_Files", "greetings_*.txt", SearchOption.AllDirectories);
            foreach (string file in files)
            {
                dbConnection.BeginTransaction();
                string[] lines = File.ReadAllLines(file);
                foreach (string line in lines)
                {
                    string[] split = line.Split('\t');

                    try
                    {
                        if (!string.IsNullOrEmpty(split[3]))
                        {
                            string token = split[3];
                            string text = split[2];
                            dbConnection.Execute($"update localized_texts set EN='{text.Replace("'", "''")}' where TOKEN='{token.Replace("'", "''")}'");
                        }
                    }
                    catch (IndexOutOfRangeException)
                    {
                    }
                }
                dbConnection.Commit();
            }

            files = Directory.GetFiles("Unity_Assets_Files", "Serena*.txt", SearchOption.AllDirectories);
            foreach (string file in files)
            {
                if (file.EndsWith("Config.txt"))
                {
                    continue;
                }

                dbConnection.BeginTransaction();
                string[] lines = File.ReadAllLines(file);
                foreach (string line in lines)
                {
                    string[] split = line.Split('#');

                    try
                    {
                        if (!string.IsNullOrEmpty(split[3]))
                        {
                            string token = split[3];
                            string text = split[2];
                            dbConnection.Execute($"update localized_texts set EN='{text.Replace("'", "''")}' where TOKEN='{token.Replace("'", "''")}'");
                        }
                    }
                    catch (IndexOutOfRangeException)
                    {
                    }
                }
                dbConnection.Commit();
            }
        }

        private static void ParseSharedAssets0TextFiles(SQLiteConnection dbConnection)
        {
            string[] files = Directory.GetFiles("Unity_Assets_Files", "gift_*.txt", SearchOption.AllDirectories);
            foreach (string file in files)
            {
                dbConnection.BeginTransaction();
                string[] lines = File.ReadAllLines(file);
                foreach (string line in lines)
                {
                    string[] split = line.Split('\t');

                    try
                    {
                        if (!string.IsNullOrEmpty(split[3]))
                        {
                            string token = split[3];
                            string text = split[2];
                            dbConnection.Execute($"update localized_texts set EN='{text.Replace("'", "''")}' where TOKEN='{token.Replace("'", "''")}'");
                        }
                    }
                    catch (IndexOutOfRangeException)
                    {
                    }
                }
                dbConnection.Commit();
            }
        }
    }
}
