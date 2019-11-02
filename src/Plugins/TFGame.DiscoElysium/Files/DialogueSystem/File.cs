namespace TFGame.DiscoElysium.Files.DialogueSystem
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using TF.Core.Files;
    using TF.Core.TranslationEntities;
    using TF.IO;
    using Yarhl.IO;
    using Yarhl.Media.Text;

    public class File : BinaryTextFileWithIds
    {
        public File(string gameName, string path, string changesFolder, System.Text.Encoding encoding) : base(gameName, path, changesFolder, encoding)
        {
        }

        protected override IList<Subtitle> GetSubtitles()
        {
            using (var fs = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fs, FileEncoding))
            {
                return GetSubtitles(input);
            }
        }

        protected IList<Subtitle> GetSubtitles(ExtendedBinaryReader input)
        {
            var result = new List<Subtitle>();

            string version = input.ReadStringSerialized(0x04);
            string author = input.ReadStringSerialized(0x04);
            string description = input.ReadStringSerialized(0x04);
            string globalUserScript = input.ReadStringSerialized(0x04);

            int emphasisSettingCount = input.ReadInt32();
            input.Skip(emphasisSettingCount * 0x04 * 0x07);

            int actorCount = input.ReadInt32();
            for (int i = 0; i < actorCount; i++)
            {
                int id = input.ReadInt32();
                int fieldCount = input.ReadInt32();

                var fields = new Dictionary<string, Field>(fieldCount);
                for (int j = 0; j < fieldCount; j++)
                {
                    var field = new Field();
                    field.Title = input.ReadStringSerialized(0x04);
                    field.Value = input.ReadStringSerialized(0x04);
                    field.Type = input.ReadInt32();
                    field.TypeString = input.ReadStringSerialized(0x04);

                    fields.Add(field.Title, field);
                }

                var translatableFields = new string[] {"Name", "LongDescription", "short_description"};
                foreach (string translatableField in translatableFields)
                {
                    if (fields.TryGetValue(translatableField, out Field field))
                    {
                        if (!string.IsNullOrEmpty(field.Value))
                        {
                            var subtitle = new SubtitleWithId
                            {
                                Id = $"Actor_{id}_{translatableField}",
                                Offset = 0,
                                Text = field.Value,
                                Loaded = field.Value,
                                Translation = field.Value
                            };

                            subtitle.PropertyChanged += SubtitlePropertyChanged;
                            result.Add(subtitle);
                        }
                    }
                }

                input.Skip(0x0C); // portrait
                input.Skip(0x04); // alternatePortraits
            }

            int itemCount = input.ReadInt32();
            for (int i = 0; i < itemCount; i++)
            {
                int id = input.ReadInt32();
                int fieldCount = input.ReadInt32();

                var fields = new Dictionary<string, Field>(fieldCount);
                for (int j = 0; j < fieldCount; j++)
                {
                    var field = new Field();
                    field.Title = input.ReadStringSerialized(0x04);
                    field.Value = input.ReadStringSerialized(0x04);
                    field.Type = input.ReadInt32();
                    field.TypeString = input.ReadStringSerialized(0x04);

                    fields.Add(field.Title, field);
                }

                var translatableFields = new string[]
                {
                    "Description", "displayname", "description", "fixtureBonus", "requirement", "bonus",
                    "fixtureDescription"
                };
                foreach (string translatableField in translatableFields)
                {
                    if (fields.TryGetValue(translatableField, out Field field))
                    {
                        if (!string.IsNullOrEmpty(field.Value))
                        {
                            var subtitle = new SubtitleWithId
                            {
                                Id = $"Item_{id}_{translatableField}",
                                Offset = 0,
                                Text = field.Value,
                                Loaded = field.Value,
                                Translation = field.Value
                            };

                            subtitle.PropertyChanged += SubtitlePropertyChanged;
                            result.Add(subtitle);
                        }
                    }
                }
            }

            int locationCount = input.ReadInt32();
            for (int i = 0; i < locationCount; i++)
            {
                int id = input.ReadInt32();
                int fieldCount = input.ReadInt32();

                var fields = new Dictionary<string, Field>(fieldCount);
                for (int j = 0; j < fieldCount; j++)
                {
                    var field = new Field();
                    field.Title = input.ReadStringSerialized(0x04);
                    field.Value = input.ReadStringSerialized(0x04);
                    field.Type = input.ReadInt32();
                    field.TypeString = input.ReadStringSerialized(0x04);

                    fields.Add(field.Title, field);
                }

                var translatableFields = new string[] {"Name"};
                foreach (string translatableField in translatableFields)
                {
                    if (fields.TryGetValue(translatableField, out Field field))
                    {
                        if (!string.IsNullOrEmpty(field.Value))
                        {
                            var subtitle = new SubtitleWithId
                            {
                                Id = $"Location_{id}_{translatableField}",
                                Offset = 0,
                                Text = field.Value,
                                Loaded = field.Value,
                                Translation = field.Value
                            };

                            subtitle.PropertyChanged += SubtitlePropertyChanged;
                            result.Add(subtitle);
                        }
                    }
                }
            }

            int variableCount = input.ReadInt32();
            for (int i = 0; i < variableCount; i++)
            {
                int id = input.ReadInt32();
                int fieldCount = input.ReadInt32();

                var fields = new Dictionary<string, Field>(fieldCount);
                for (int j = 0; j < fieldCount; j++)
                {
                    var field = new Field();
                    field.Title = input.ReadStringSerialized(0x04);
                    field.Value = input.ReadStringSerialized(0x04);
                    field.Type = input.ReadInt32();
                    field.TypeString = input.ReadStringSerialized(0x04);

                    fields.Add(field.Title, field);
                }

                var translatableFields = new string[] {"Description"};
                foreach (string translatableField in translatableFields)
                {
                    if (fields.TryGetValue(translatableField, out Field field))
                    {
                        if (!string.IsNullOrEmpty(field.Value))
                        {
                            var subtitle = new SubtitleWithId
                            {
                                Id = $"Variable_{id}_{translatableField}",
                                Offset = 0,
                                Text = field.Value,
                                Loaded = field.Value,
                                Translation = field.Value
                            };

                            subtitle.PropertyChanged += SubtitlePropertyChanged;
                            result.Add(subtitle);
                        }
                    }
                }
            }

            int conversationCount = input.ReadInt32();
            for (int i = 0; i < conversationCount; i++)
            {
                int id = input.ReadInt32();
                int fieldCount = input.ReadInt32();

                var fields = new Dictionary<string, Field>(fieldCount);
                for (int j = 0; j < fieldCount; j++)
                {
                    var field = new Field();
                    field.Title = input.ReadStringSerialized(0x04);
                    field.Value = input.ReadStringSerialized(0x04);
                    field.Type = input.ReadInt32();
                    field.TypeString = input.ReadStringSerialized(0x04);

                    fields.Add(field.Title, field);
                }

                var translatableFields = new string[]
                {
                    "Title", "Description"
                }; //, "subtask_title_01", "subtask_title_02", "subtask_title_03", "subtask_title_04", "subtask_title_05", "subtask_title_06", "subtask_title_07", "subtask_title_08", "subtask_title_09", "subtask_title_10" };
                foreach (string translatableField in translatableFields)
                {
                    if (fields.TryGetValue(translatableField, out Field field))
                    {
                        if (!string.IsNullOrEmpty(field.Value))
                        {
                            var subtitle = new SubtitleWithId
                            {
                                Id = $"Conversation_{id}_{translatableField}",
                                Offset = 0,
                                Text = field.Value,
                                Loaded = field.Value,
                                Translation = field.Value
                            };

                            subtitle.PropertyChanged += SubtitlePropertyChanged;
                            result.Add(subtitle);
                        }
                    }
                }

                // ConversationOverrideDisplaySettings
                input.Skip(0x28);
                for (int j = 0; j < 3; j++)
                {
                    input.ReadStringSerialized(0x04);
                }

                input.Skip(0x10);

                input.ReadStringSerialized(0x04); // nodeColor

                // DialogueEntry
                int dialogueEntryCount = input.ReadInt32();
                for (int j = 0; j < dialogueEntryCount; j++)
                {
                    int dialogueEntryId = input.ReadInt32();
                    int fieldCount2 = input.ReadInt32();

                    var fields2 = new Dictionary<string, Field>(fieldCount2);
                    for (int k = 0; k < fieldCount2; k++)
                    {
                        var field = new Field();
                        field.Title = input.ReadStringSerialized(0x04);
                        field.Value = input.ReadStringSerialized(0x04);
                        field.Type = input.ReadInt32();
                        field.TypeString = input.ReadStringSerialized(0x04);

                        fields2.Add($"{field.Title}", field);
                    }

                    var translatableFields2 = new string[]
                    {
                        "Dialogue Text", "Alternate1", "Alternate2", "Alternate3", "Alternate4", "tooltip1", "tooltip2",
                        "tooltip3", "tooltip4", "tooltip5", "tooltip6", "tooltip7", "tooltip8", "tooltip9", "tooltip10"
                    };
                    foreach (string translatableField in translatableFields2)
                    {
                        if (fields2.TryGetValue(translatableField, out Field field))
                        {
                            if (!string.IsNullOrEmpty(field.Value))
                            {
                                var subtitle = new SubtitleWithId
                                {
                                    Id = $"Conversation_{id}_Entry_{dialogueEntryId}_{translatableField}",
                                    Offset = 0,
                                    Text = field.Value,
                                    Loaded = field.Value,
                                    Translation = field.Value
                                };

                                subtitle.PropertyChanged += SubtitlePropertyChanged;
                                result.Add(subtitle);
                            }
                        }
                    }

                    input.Skip(0x0C);
                    input.ReadStringSerialized(0x04); // nodeColor
                    input.Skip(0x04);
                    input.ReadStringSerialized(0x04); // falseConditionAction
                    input.Skip(0x04);
                    int outgoingLinksCount = input.ReadInt32();
                    input.Skip(outgoingLinksCount * 0x04 * 0x06);

                    input.ReadStringSerialized(0x04); // conditionsString
                    string userScript = input.ReadStringSerialized(0x04); // userScript

                    if (userScript.StartsWith("NewspaperEndgame("))
                    {
                        var regex = new Regex(@"NewspaperEndgame\(""(?<Id>[^""]+)"",""(?<Headline>[^""]+)"",""(?<Text>[^""\\]*(?:\\.[^""\\]*)*)""\)");
                        Match match = regex.Match(userScript);

                        while (match.Success)
                        {
                            string endId = match.Groups["Id"].Value;
                            string headline = match.Groups["Headline"].Value;
                            string text = match.Groups["Text"].Value;
                        
                            var subtitle = new SubtitleWithId
                            {
                                Id = $"Conversation_{id}_Entry_{dialogueEntryId}_EndHeadline",
                                Offset = 0,
                                Text = headline,
                                Loaded = headline,
                                Translation = headline
                            };

                            subtitle.PropertyChanged += SubtitlePropertyChanged;
                            result.Add(subtitle);

                            text = text.Replace("<NewLine>", "\\n").Replace("\\\"", "\"");
                            subtitle = new SubtitleWithId
                            {
                                Id = $"Conversation_{id}_Entry_{dialogueEntryId}_EndText",
                                Offset = 0,
                                Text = text,
                                Loaded = text,
                                Translation = text
                            };

                            subtitle.PropertyChanged += SubtitlePropertyChanged;
                            result.Add(subtitle);    

                            match = match.NextMatch();
                        }
                    }

                    // onExecute
                    input.Skip(0x04);
                    input.ReadStringSerialized(0x04); // typeName

                    input.Skip(0x10); // Canvas rect
                }

                input.Skip(0x08); // canvasScrollPosition
                input.Skip(0x04); // canvasZoom
            }

            LoadChanges(result);

            return result;
        }

        public override void Rebuild(string outputFolder)
        {
            string outputPath = System.IO.Path.Combine(outputFolder, RelativePath);
            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(outputPath));

            IList<Subtitle> subtitles = GetSubtitles();
            List<SubtitleWithId> subs = subtitles.Select(subtitle => subtitle as SubtitleWithId).ToList();
            var dictionary = new Dictionary<string, SubtitleWithId>(subs.Count);
            foreach (SubtitleWithId subtitle in subs)
            {
                dictionary.Add(subtitle.Id, subtitle);
            }

            using (var fsInput = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fsInput, FileEncoding))
            using (var fsOutput = new FileStream(outputPath, FileMode.Create))
            using (var output = new ExtendedBinaryWriter(fsOutput, FileEncoding))
            {
                Rebuild(input, output, dictionary);
            }
        }

        protected void Rebuild(ExtendedBinaryReader input, ExtendedBinaryWriter output,
            Dictionary<string, SubtitleWithId> dictionary)
        {
            string version = input.ReadStringSerialized(0x04);
            output.WriteStringSerialized(version, 0x04);
            string author = input.ReadStringSerialized(0x04);
            output.WriteStringSerialized(author, 0x04);
            string description = input.ReadStringSerialized(0x04);
            output.WriteStringSerialized(description, 0x04);
            string globalUserScript = input.ReadStringSerialized(0x04);
            output.WriteStringSerialized(globalUserScript, 0x04);

            int emphasisSettingCount = input.ReadInt32();
            output.Write(emphasisSettingCount);
            output.Write(input.ReadBytes(emphasisSettingCount * 0x04 * 0x07));

            int actorCount = input.ReadInt32();
            output.Write(actorCount);
            for (int i = 0; i < actorCount; i++)
            {
                int id = input.ReadInt32();
                output.Write(id);
                int fieldCount = input.ReadInt32();
                output.Write(fieldCount);

                var fields = new Dictionary<string, Field>(fieldCount);
                for (int j = 0; j < fieldCount; j++)
                {
                    var field = new Field();
                    field.Title = input.ReadStringSerialized(0x04);
                    field.Value = input.ReadStringSerialized(0x04);
                    field.Type = input.ReadInt32();
                    field.TypeString = input.ReadStringSerialized(0x04);

                    fields.Add(field.Title, field);
                }

                var translatableFields = new string[] {"Name", "LongDescription", "short_description"};
                foreach (KeyValuePair<string, Field> kvp in fields)
                {
                    output.WriteStringSerialized(kvp.Value.Title, 0x04);
                    if (translatableFields.Contains(kvp.Value.Title) && !string.IsNullOrEmpty(kvp.Value.Value))
                    {
                        string key = $"Actor_{id}_{kvp.Value.Title}";
                        SubtitleWithId subtitle = dictionary[key];
                        output.WriteStringSerialized(subtitle.Translation, 0x04);
                    }
                    else
                    {
                        output.WriteStringSerialized(kvp.Value.Value, 0x04);
                    }

                    output.Write(kvp.Value.Type);
                    output.WriteStringSerialized(kvp.Value.TypeString, 0x04);
                }

                output.Write(input.ReadBytes(0x0C)); // portrait
                output.Write(input.ReadBytes(0x04)); // alternatePortraits
            }

            int itemCount = input.ReadInt32();
            output.Write(itemCount);
            for (int i = 0; i < itemCount; i++)
            {
                int id = input.ReadInt32();
                output.Write(id);
                int fieldCount = input.ReadInt32();
                output.Write(fieldCount);

                var fields = new Dictionary<string, Field>(fieldCount);
                for (int j = 0; j < fieldCount; j++)
                {
                    var field = new Field();
                    field.Title = input.ReadStringSerialized(0x04);
                    field.Value = input.ReadStringSerialized(0x04);
                    field.Type = input.ReadInt32();
                    field.TypeString = input.ReadStringSerialized(0x04);

                    fields.Add(field.Title, field);
                }

                var translatableFields = new string[]
                {
                    "Description", "displayname", "description", "fixtureBonus", "requirement", "bonus",
                    "fixtureDescription"
                };
                foreach (KeyValuePair<string, Field> kvp in fields)
                {
                    output.WriteStringSerialized(kvp.Value.Title, 0x04);
                    if (translatableFields.Contains(kvp.Value.Title) && !string.IsNullOrEmpty(kvp.Value.Value))
                    {
                        string key = $"Item_{id}_{kvp.Value.Title}";
                        SubtitleWithId subtitle = dictionary[key];
                        output.WriteStringSerialized(subtitle.Translation, 0x04);
                    }
                    else
                    {
                        output.WriteStringSerialized(kvp.Value.Value, 0x04);
                    }

                    output.Write(kvp.Value.Type);
                    output.WriteStringSerialized(kvp.Value.TypeString, 0x04);
                }
            }

            int locationCount = input.ReadInt32();
            output.Write(locationCount);
            for (int i = 0; i < locationCount; i++)
            {
                int id = input.ReadInt32();
                output.Write(id);
                int fieldCount = input.ReadInt32();
                output.Write(fieldCount);

                var fields = new Dictionary<string, Field>(fieldCount);
                for (int j = 0; j < fieldCount; j++)
                {
                    var field = new Field();
                    field.Title = input.ReadStringSerialized(0x04);
                    field.Value = input.ReadStringSerialized(0x04);
                    field.Type = input.ReadInt32();
                    field.TypeString = input.ReadStringSerialized(0x04);

                    fields.Add(field.Title, field);
                }

                var translatableFields = new string[] {"Name"};
                foreach (KeyValuePair<string, Field> kvp in fields)
                {
                    output.WriteStringSerialized(kvp.Value.Title, 0x04);
                    if (translatableFields.Contains(kvp.Value.Title) && !string.IsNullOrEmpty(kvp.Value.Value))
                    {
                        string key = $"Location_{id}_{kvp.Value.Title}";
                        SubtitleWithId subtitle = dictionary[key];
                        output.WriteStringSerialized(subtitle.Translation, 0x04);
                    }
                    else
                    {
                        output.WriteStringSerialized(kvp.Value.Value, 0x04);
                    }

                    output.Write(kvp.Value.Type);
                    output.WriteStringSerialized(kvp.Value.TypeString, 0x04);
                }
            }

            int variableCount = input.ReadInt32();
            output.Write(variableCount);
            for (int i = 0; i < variableCount; i++)
            {
                int id = input.ReadInt32();
                output.Write(id);
                int fieldCount = input.ReadInt32();
                output.Write(fieldCount);

                var fields = new Dictionary<string, Field>(fieldCount);
                for (int j = 0; j < fieldCount; j++)
                {
                    var field = new Field();
                    field.Title = input.ReadStringSerialized(0x04);
                    field.Value = input.ReadStringSerialized(0x04);
                    field.Type = input.ReadInt32();
                    field.TypeString = input.ReadStringSerialized(0x04);

                    fields.Add(field.Title, field);
                }

                var translatableFields = new string[] {"Description"};
                foreach (KeyValuePair<string, Field> kvp in fields)
                {
                    output.WriteStringSerialized(kvp.Value.Title, 0x04);
                    if (translatableFields.Contains(kvp.Value.Title) && !string.IsNullOrEmpty(kvp.Value.Value))
                    {
                        string key = $"Variable_{id}_{kvp.Value.Title}";
                        SubtitleWithId subtitle = dictionary[key];
                        output.WriteStringSerialized(subtitle.Translation, 0x04);
                    }
                    else
                    {
                        output.WriteStringSerialized(kvp.Value.Value, 0x04);
                    }

                    output.Write(kvp.Value.Type);
                    output.WriteStringSerialized(kvp.Value.TypeString, 0x04);
                }
            }

            int conversationCount = input.ReadInt32();
            output.Write(conversationCount);
            for (int i = 0; i < conversationCount; i++)
            {
                int id = input.ReadInt32();
                output.Write(id);
                int fieldCount = input.ReadInt32();
                output.Write(fieldCount);

                var fields = new Dictionary<string, Field>(fieldCount);
                for (int j = 0; j < fieldCount; j++)
                {
                    var field = new Field();
                    field.Title = input.ReadStringSerialized(0x04);
                    field.Value = input.ReadStringSerialized(0x04);
                    field.Type = input.ReadInt32();
                    field.TypeString = input.ReadStringSerialized(0x04);

                    fields.Add(field.Title, field);
                }

                var translatableFields = new string[]
                {
                    "Title", "Description"
                }; //, "subtask_title_01", "subtask_title_02", "subtask_title_03", "subtask_title_04", "subtask_title_05", "subtask_title_06", "subtask_title_07", "subtask_title_08", "subtask_title_09", "subtask_title_10" };
                foreach (KeyValuePair<string, Field> kvp in fields)
                {
                    output.WriteStringSerialized(kvp.Value.Title, 0x04);
                    if (translatableFields.Contains(kvp.Value.Title) && !string.IsNullOrEmpty(kvp.Value.Value))
                    {
                        string key = $"Conversation_{id}_{kvp.Value.Title}";
                        SubtitleWithId subtitle = dictionary[key];
                        output.WriteStringSerialized(subtitle.Translation, 0x04);
                    }
                    else
                    {
                        output.WriteStringSerialized(kvp.Value.Value, 0x04);
                    }

                    output.Write(kvp.Value.Type);
                    output.WriteStringSerialized(kvp.Value.TypeString, 0x04);
                }

                // ConversationOverrideDisplaySettings
                output.Write(input.ReadBytes(0x28));
                for (int j = 0; j < 3; j++)
                {
                    output.WriteStringSerialized(input.ReadStringSerialized(0x04), 0x04);
                }

                output.Write(input.ReadBytes(0x10));

                output.WriteStringSerialized(input.ReadStringSerialized(0x04), 0x04); // nodeColor

                // DialogueEntry
                int dialogueEntryCount = input.ReadInt32();
                output.Write(dialogueEntryCount);
                for (int j = 0; j < dialogueEntryCount; j++)
                {
                    int dialogueEntryId = input.ReadInt32();
                    output.Write(dialogueEntryId);
                    int fieldCount2 = input.ReadInt32();
                    output.Write(fieldCount2);

                    var fields2 = new Dictionary<string, Field>(fieldCount2);
                    for (int k = 0; k < fieldCount2; k++)
                    {
                        var field = new Field();
                        field.Title = input.ReadStringSerialized(0x04);
                        field.Value = input.ReadStringSerialized(0x04);
                        field.Type = input.ReadInt32();
                        field.TypeString = input.ReadStringSerialized(0x04);

                        fields2.Add($"{field.Title}", field);
                    }

                    var translatableFields2 = new string[]
                    {
                        "Dialogue Text", "Alternate1", "Alternate2", "Alternate3", "Alternate4", "tooltip1", "tooltip2",
                        "tooltip3", "tooltip4", "tooltip5", "tooltip6", "tooltip7", "tooltip8", "tooltip9", "tooltip10"
                    };
                    foreach (KeyValuePair<string, Field> kvp in fields2)
                    {
                        output.WriteStringSerialized(kvp.Value.Title, 0x04);
                        if (translatableFields2.Contains(kvp.Value.Title) && !string.IsNullOrEmpty(kvp.Value.Value))
                        {
                            string key = $"Conversation_{id}_Entry_{dialogueEntryId}_{kvp.Value.Title}";
                            SubtitleWithId subtitle = dictionary[key];
                            output.WriteStringSerialized(subtitle.Translation, 0x04);
                        }
                        else
                        {
                            output.WriteStringSerialized(kvp.Value.Value, 0x04);
                        }

                        output.Write(kvp.Value.Type);
                        output.WriteStringSerialized(kvp.Value.TypeString, 0x04);
                    }

                    output.Write(input.ReadBytes(0x0C));
                    output.WriteStringSerialized(input.ReadStringSerialized(0x04), 0x04); // nodeColor
                    output.Write(input.ReadBytes(0x04));
                    output.WriteStringSerialized(input.ReadStringSerialized(0x04), 0x04); // falseConditionAction
                    output.Write(input.ReadBytes(0x04));

                    int outgoingLinksCount = input.ReadInt32();
                    output.Write(outgoingLinksCount);
                    output.Write(input.ReadBytes(outgoingLinksCount * 0x04 * 0x06));

                    output.WriteStringSerialized(input.ReadStringSerialized(0x04), 0x04); // conditionsString
                    string userScript = input.ReadStringSerialized(0x04); // userScript

                    if (userScript.StartsWith("NewspaperEndgame("))
                    {
                        var regex = new Regex(@"NewspaperEndgame\(""(?<Id>[^""]+)"",""(?<Headline>[^""]+)"",""(?<Text>[^""\\]*(?:\\.[^""\\]*)*)""\)");
                        Match match = regex.Match(userScript);

                        while (match.Success)
                        {
                            string endId = match.Groups["Id"].Value;

                            string headlineKey = $"Conversation_{id}_Entry_{dialogueEntryId}_EndHeadline";
                            string textKey = $"Conversation_{id}_Entry_{dialogueEntryId}_EndText";

                            if (!dictionary.ContainsKey(headlineKey) || !dictionary.ContainsKey(textKey))
                            {
                                output.WriteStringSerialized(userScript, 0x04);
                            }
                            else
                            {
                                SubtitleWithId headline = dictionary[headlineKey];
                                SubtitleWithId text = dictionary[textKey];

                                string escapedText = text.Translation.Replace("\"", "\\\"").Replace("\\n", "<NewLine>");
                                output.WriteStringSerialized($@"NewspaperEndgame(""{endId}"",""{headline}"",""{escapedText}"")", 0x04);
                            }

                            match = match.NextMatch();
                        }
                    }
                    else
                    {
                        output.WriteStringSerialized(userScript, 0x04);
                    }

                    // onExecute
                    output.Write(input.ReadBytes(0x04));
                    output.WriteStringSerialized(input.ReadStringSerialized(0x04), 0x04); // typeName

                    output.Write(input.ReadBytes(0x10)); // Canvas rect
                }

                output.Write(input.ReadBytes(0x08)); // canvasScrollPosition
                output.Write(input.ReadBytes(0x04)); // canvasZoom
            }

            int remainderLength = (int) (input.Length - input.Position);
            byte[] remainder = input.ReadBytes(remainderLength);
            output.Write(remainder);
        }

        public override void ExportPo(string path)
        {
            string directory = System.IO.Path.GetDirectoryName(path);
            Directory.CreateDirectory(directory);
            
            var po = new Po()
            {
                Header = new PoHeader(GameName, "dummy@dummy.com", "es-ES")
            };

            IList<Subtitle> subtitles = GetSubtitles();

            string ctx = GetContext(subtitles[0]);
            string[] tags = ctx.Split('_');
            string previousTag0 = tags[0];
            string previousTag1 = tags[1];

            int index = 1;

            var po2binary = new Yarhl.Media.Text.Po2Binary();

            string outputFolder = System.IO.Path.GetDirectoryName(path);
            string fileName = System.IO.Path.GetFileNameWithoutExtension(path);

            foreach (Subtitle subtitle in subtitles)
            {
                var entry = new PoEntry();

                string original = subtitle.Text;
                string translation = subtitle.Translation;
                if (string.IsNullOrEmpty(original))
                {
                    original = "<!empty>";
                    translation = "<!empty>";
                }

                entry.Original = original.Replace(LineEnding.ShownLineEnding, LineEnding.PoLineEnding);
                entry.Context = GetContext(subtitle);

                if (original != translation)
                {
                    entry.Translated = translation.Replace(LineEnding.ShownLineEnding, LineEnding.PoLineEnding);
                }

                tags = entry.Context.Split('_');
                string tag0 = tags[0];
                string tag1 = tags[1];
                if (tag0 != previousTag0)
                {
                    BinaryFormat outputBinary = po2binary.Convert(po);
                    string outputPath = System.IO.Path.Combine(outputFolder, $"{fileName}.{previousTag0}.po");
                    outputBinary.Stream.WriteTo(outputPath);

                    po = new Po()
                    {
                        Header = new PoHeader(GameName, "dummy@dummy.com", "es-ES")
                    };

                }
                else if (tag0 == "Conversation" && tag1 != previousTag1 && po.Entries.Count >= 1000)
                {
                    BinaryFormat outputBinary = po2binary.Convert(po);
                    string outputPath = System.IO.Path.Combine(outputFolder, $"{fileName}.{previousTag0}_{index}.po");
                    outputBinary.Stream.WriteTo(outputPath);

                    po = new Po()
                    {
                        Header = new PoHeader(GameName, "dummy@dummy.com", "es-ES")
                    };

                    index++;
                }

                po.Add(entry);

                previousTag0 = tag0;
                previousTag1 = tag1;
            }

            BinaryFormat binary = po2binary.Convert(po);
            string path1 = System.IO.Path.Combine(outputFolder, $"{fileName}.{previousTag0}_{index}.po");
            binary.Stream.WriteTo(path1);
        }

        private class Field
        {
            public string Title { get; set; }
            public string Value { get; set; }
            public int Type { get; set; }
            public string TypeString { get; set; }
        }
    }
}
