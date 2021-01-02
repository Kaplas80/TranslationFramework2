namespace LoveEsquireVnTextBuilder
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Serialization;
    using LoveEsquireVnTextBuilder.XmlClasses;
    using SQLite;

    class Program
    {
        static void Main(string[] args)
        {
            string gameFolder = @"Z:\GAMES\Love Esquire\Love Esquire_Data\";

            string vntextPath = string.Concat(gameFolder, @"StreamingAssets\SqlText\vntext.sq");
            File.Copy(vntextPath, "vntext.sq", true);

            string vnscenesFolder = string.Concat(gameFolder, @"StreamingAssets\Windows\vnscenes");
            string[] vnfiles = Directory.GetFiles(vnscenesFolder, "*.");

            string resources = string.Concat(gameFolder, "resources.assets");
            string sharedAssets0 = string.Concat(gameFolder, "sharedassets0.assets");
            string campaign = string.Concat(gameFolder, @"StreamingAssets\Windows\campaign");

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

            fileName = Path.GetFileName(campaign);
            File.Copy(campaign, fileName, true);
            RunUnityEx(fileName);
            ParseCampaignTextFiles(dbConnection);
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
                            InsertData(token, text, dbConnection);
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
                            InsertData(token, text, dbConnection);
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
                            InsertData(token, text, dbConnection);
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
                            InsertData(token, text, dbConnection);
                        }
                    }
                    catch (IndexOutOfRangeException)
                    {
                    }
                }
                dbConnection.Commit();
            }
        }

        private static void ParseCampaignTextFiles(SQLiteConnection dbConnection)
        {
            string[] files = Directory.GetFiles("Unity_Assets_Files", "BanterData.xml", SearchOption.AllDirectories);
            foreach (string file in files)
            {
                string textData = File.ReadAllText(file);
                var textReader = new StringReader(textData);
                var xmlSerializer = new XmlSerializer(typeof(BanterData));
                var xml = (BanterData)xmlSerializer.Deserialize(textReader);

                dbConnection.BeginTransaction();
                foreach (BanterGroup dataGroup in xml.banterGroups)
                {
                    foreach (Dialogue data in dataGroup.starterDialogues)
                    {
                        string token = data.va;
                        string text = data.line;
                        InsertData(token, text, dbConnection);
                    }

                    foreach (Dialogue data in dataGroup.responseDialogues)
                    {
                        string token = data.va;
                        string text = data.line;
                        InsertData(token, text, dbConnection);
                    }
                }
                dbConnection.Commit();
            }

            files = Directory.GetFiles("Unity_Assets_Files", "CampaignMapTalkData.xml", SearchOption.AllDirectories);
            foreach (string file in files)
            {
                string textData = File.ReadAllText(file);
                var textReader = new StringReader(textData);
                var xmlSerializer = new XmlSerializer(typeof(CampaignMapTalkData));
                var xml = (CampaignMapTalkData)xmlSerializer.Deserialize(textReader);

                dbConnection.BeginTransaction();
                foreach (CampaignMapTalkGroup dataGroup in xml.talkGroups)
                {
                    foreach (CampaignMapTalk data in dataGroup.campaignMapTalks)
                    {
                        string token = data.vaFile;
                        string text = data.text;
                        InsertData(token, text, dbConnection);
                    }
                }
                dbConnection.Commit();
            }

            files = Directory.GetFiles("Unity_Assets_Files", "CampaignS*.xml", SearchOption.AllDirectories);
            foreach (string file in files)
            {
                string textData = File.ReadAllText(file);
                var textReader = new StringReader(textData);
                var xmlSerializer = new XmlSerializer(typeof(DialogueData));
                var xml = (DialogueData)xmlSerializer.Deserialize(textReader);

                dbConnection.BeginTransaction();
                foreach (DialogueSet dataGroup in xml.dialogueGroups)
                {
                    foreach (Dialogue data in dataGroup.dialogues)
                    {
                        string token = data.va;
                        string text = data.line;
                        InsertData(token, text, dbConnection);
                    }
                }
                dbConnection.Commit();
            }
        }

        private static void InsertData(string token, string text, SQLiteConnection dbConnection)
        {
            if (string.IsNullOrEmpty(token))
            {
                return;
            }

            string escapedToken = token.Replace("'", "''");
            string escapedText = text.Replace("'", "''");
            int count = dbConnection.ExecuteScalar<int>($"select count(*) from localized_texts where TOKEN='{escapedToken}'");
            dbConnection.Execute(count > 0
                ? $"update localized_texts set EN='{escapedText}' where TOKEN='{escapedToken}'"
                : $"insert into localized_texts(TOKEN, EN) values ('{escapedToken}', '{escapedText}')");
        }
    }
}
