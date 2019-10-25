namespace TFGame.DiscoElysium.Files.DialogueSystem
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using TF.Core.TranslationEntities;
    using TF.IO;
    using TFGame.DiscoElysium.Files.Common;

    public class File : DiscoElysiumTextFile
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

            var version = input.ReadStringSerialized(0x04);
            var author = input.ReadStringSerialized(0x04);
            var description = input.ReadStringSerialized(0x04);
            var globalUserScript = input.ReadStringSerialized(0x04);

            var emphasisSettingCount = input.ReadInt32();
            input.Skip(emphasisSettingCount * 0x04 * 0x07);

            var actorCount = input.ReadInt32();
            for (var i = 0; i < actorCount; i++)
            {
                var id = input.ReadInt32();
                var fieldCount = input.ReadInt32();

                var fields = new Dictionary<string, Field>(fieldCount);
                for (var j = 0; j < fieldCount; j++)
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
                            var subtitle = new DiscoElysiumSubtitle
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

            var itemCount = input.ReadInt32();
            for (var i = 0; i < itemCount; i++)
            {
                var id = input.ReadInt32();
                var fieldCount = input.ReadInt32();

                var fields = new Dictionary<string, Field>(fieldCount);
                for (var j = 0; j < fieldCount; j++)
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
                            var subtitle = new DiscoElysiumSubtitle
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

            var locationCount = input.ReadInt32();
            for (var i = 0; i < locationCount; i++)
            {
                var id = input.ReadInt32();
                var fieldCount = input.ReadInt32();

                var fields = new Dictionary<string, Field>(fieldCount);
                for (var j = 0; j < fieldCount; j++)
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
                            var subtitle = new DiscoElysiumSubtitle
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

            var variableCount = input.ReadInt32();
            for (var i = 0; i < variableCount; i++)
            {
                var id = input.ReadInt32();
                var fieldCount = input.ReadInt32();

                var fields = new Dictionary<string, Field>(fieldCount);
                for (var j = 0; j < fieldCount; j++)
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
                            var subtitle = new DiscoElysiumSubtitle
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

            var conversationCount = input.ReadInt32();
            for (var i = 0; i < conversationCount; i++)
            {
                var id = input.ReadInt32();
                var fieldCount = input.ReadInt32();

                var fields = new Dictionary<string, Field>(fieldCount);
                for (var j = 0; j < fieldCount; j++)
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
                            var subtitle = new DiscoElysiumSubtitle
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
                for (var j = 0; j < 3; j++)
                {
                    input.ReadStringSerialized(0x04);
                }

                input.Skip(0x10);

                input.ReadStringSerialized(0x04); // nodeColor

                // DialogueEntry
                var dialogueEntryCount = input.ReadInt32();
                for (var j = 0; j < dialogueEntryCount; j++)
                {
                    var dialogueEntryId = input.ReadInt32();
                    var fieldCount2 = input.ReadInt32();

                    var fields2 = new Dictionary<string, Field>(fieldCount2);
                    for (var k = 0; k < fieldCount2; k++)
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
                                var subtitle = new DiscoElysiumSubtitle
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
                    var outgoingLinksCount = input.ReadInt32();
                    input.Skip(outgoingLinksCount * 0x04 * 0x06);

                    input.ReadStringSerialized(0x04); // conditionsString
                    var userScript = input.ReadStringSerialized(0x04); // userScript

                    // TODO: Comprobar si hay que traducir el userScript

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
            var outputPath = System.IO.Path.Combine(outputFolder, RelativePath);
            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(outputPath));

            var subtitles = GetSubtitles();
            var subs = subtitles.Select(subtitle => subtitle as DiscoElysiumSubtitle).ToList();
            var dictionary = new Dictionary<string, DiscoElysiumSubtitle>(subs.Count);
            foreach (DiscoElysiumSubtitle subtitle in subs)
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
            Dictionary<string, DiscoElysiumSubtitle> dictionary)
        {
            var version = input.ReadStringSerialized(0x04);
            output.WriteStringSerialized(version, 0x04);
            var author = input.ReadStringSerialized(0x04);
            output.WriteStringSerialized(author, 0x04);
            var description = input.ReadStringSerialized(0x04);
            output.WriteStringSerialized(description, 0x04);
            var globalUserScript = input.ReadStringSerialized(0x04);
            output.WriteStringSerialized(globalUserScript, 0x04);

            var emphasisSettingCount = input.ReadInt32();
            output.Write(emphasisSettingCount);
            output.Write(input.ReadBytes(emphasisSettingCount * 0x04 * 0x07));

            var actorCount = input.ReadInt32();
            output.Write(actorCount);
            for (var i = 0; i < actorCount; i++)
            {
                var id = input.ReadInt32();
                output.Write(id);
                var fieldCount = input.ReadInt32();
                output.Write(fieldCount);

                var fields = new Dictionary<string, Field>(fieldCount);
                for (var j = 0; j < fieldCount; j++)
                {
                    var field = new Field();
                    field.Title = input.ReadStringSerialized(0x04);
                    field.Value = input.ReadStringSerialized(0x04);
                    field.Type = input.ReadInt32();
                    field.TypeString = input.ReadStringSerialized(0x04);

                    fields.Add(field.Title, field);
                }

                var translatableFields = new string[] {"Name", "LongDescription", "short_description"};
                foreach (var kvp in fields)
                {
                    output.WriteStringSerialized(kvp.Value.Title, 0x04);
                    if (translatableFields.Contains(kvp.Value.Title) && !string.IsNullOrEmpty(kvp.Value.Value))
                    {
                        var key = $"Actor_{id}_{kvp.Value.Title}";
                        var subtitle = dictionary[key];
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

            var itemCount = input.ReadInt32();
            output.Write(itemCount);
            for (var i = 0; i < itemCount; i++)
            {
                var id = input.ReadInt32();
                output.Write(id);
                var fieldCount = input.ReadInt32();
                output.Write(fieldCount);

                var fields = new Dictionary<string, Field>(fieldCount);
                for (var j = 0; j < fieldCount; j++)
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
                foreach (var kvp in fields)
                {
                    output.WriteStringSerialized(kvp.Value.Title, 0x04);
                    if (translatableFields.Contains(kvp.Value.Title) && !string.IsNullOrEmpty(kvp.Value.Value))
                    {
                        var key = $"Item_{id}_{kvp.Value.Title}";
                        var subtitle = dictionary[key];
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

            var locationCount = input.ReadInt32();
            output.Write(locationCount);
            for (var i = 0; i < locationCount; i++)
            {
                var id = input.ReadInt32();
                output.Write(id);
                var fieldCount = input.ReadInt32();
                output.Write(fieldCount);

                var fields = new Dictionary<string, Field>(fieldCount);
                for (var j = 0; j < fieldCount; j++)
                {
                    var field = new Field();
                    field.Title = input.ReadStringSerialized(0x04);
                    field.Value = input.ReadStringSerialized(0x04);
                    field.Type = input.ReadInt32();
                    field.TypeString = input.ReadStringSerialized(0x04);

                    fields.Add(field.Title, field);
                }

                var translatableFields = new string[] {"Name"};
                foreach (var kvp in fields)
                {
                    output.WriteStringSerialized(kvp.Value.Title, 0x04);
                    if (translatableFields.Contains(kvp.Value.Title) && !string.IsNullOrEmpty(kvp.Value.Value))
                    {
                        var key = $"Location_{id}_{kvp.Value.Title}";
                        var subtitle = dictionary[key];
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

            var variableCount = input.ReadInt32();
            output.Write(variableCount);
            for (var i = 0; i < variableCount; i++)
            {
                var id = input.ReadInt32();
                output.Write(id);
                var fieldCount = input.ReadInt32();
                output.Write(fieldCount);

                var fields = new Dictionary<string, Field>(fieldCount);
                for (var j = 0; j < fieldCount; j++)
                {
                    var field = new Field();
                    field.Title = input.ReadStringSerialized(0x04);
                    field.Value = input.ReadStringSerialized(0x04);
                    field.Type = input.ReadInt32();
                    field.TypeString = input.ReadStringSerialized(0x04);

                    fields.Add(field.Title, field);
                }

                var translatableFields = new string[] {"Description"};
                foreach (var kvp in fields)
                {
                    output.WriteStringSerialized(kvp.Value.Title, 0x04);
                    if (translatableFields.Contains(kvp.Value.Title) && !string.IsNullOrEmpty(kvp.Value.Value))
                    {
                        var key = $"Variable_{id}_{kvp.Value.Title}";
                        var subtitle = dictionary[key];
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

            var conversationCount = input.ReadInt32();
            output.Write(conversationCount);
            for (var i = 0; i < conversationCount; i++)
            {
                var id = input.ReadInt32();
                output.Write(id);
                var fieldCount = input.ReadInt32();
                output.Write(fieldCount);

                var fields = new Dictionary<string, Field>(fieldCount);
                for (var j = 0; j < fieldCount; j++)
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
                foreach (var kvp in fields)
                {
                    output.WriteStringSerialized(kvp.Value.Title, 0x04);
                    if (translatableFields.Contains(kvp.Value.Title) && !string.IsNullOrEmpty(kvp.Value.Value))
                    {
                        var key = $"Conversation_{id}_{kvp.Value.Title}";
                        var subtitle = dictionary[key];
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
                for (var j = 0; j < 3; j++)
                {
                    output.WriteStringSerialized(input.ReadStringSerialized(0x04), 0x04);
                }

                output.Write(input.ReadBytes(0x10));

                output.WriteStringSerialized(input.ReadStringSerialized(0x04), 0x04); // nodeColor

                // DialogueEntry
                var dialogueEntryCount = input.ReadInt32();
                output.Write(dialogueEntryCount);
                for (var j = 0; j < dialogueEntryCount; j++)
                {
                    var dialogueEntryId = input.ReadInt32();
                    output.Write(dialogueEntryId);
                    var fieldCount2 = input.ReadInt32();
                    output.Write(fieldCount2);

                    var fields2 = new Dictionary<string, Field>(fieldCount2);
                    for (var k = 0; k < fieldCount2; k++)
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
                    foreach (var kvp in fields2)
                    {
                        output.WriteStringSerialized(kvp.Value.Title, 0x04);
                        if (translatableFields2.Contains(kvp.Value.Title) && !string.IsNullOrEmpty(kvp.Value.Value))
                        {
                            var key = $"Conversation_{id}_Entry_{dialogueEntryId}_{kvp.Value.Title}";
                            var subtitle = dictionary[key];
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

                    var outgoingLinksCount = input.ReadInt32();
                    output.Write(outgoingLinksCount);
                    output.Write(input.ReadBytes(outgoingLinksCount * 0x04 * 0x06));

                    output.WriteStringSerialized(input.ReadStringSerialized(0x04), 0x04); // conditionsString
                    var userScript = input.ReadStringSerialized(0x04); // userScript
                    output.WriteStringSerialized(userScript, 0x04);
                    // TODO: Comprobar si hay que traducir el userScript

                    // onExecute
                    output.Write(input.ReadBytes(0x04));
                    output.WriteStringSerialized(input.ReadStringSerialized(0x04), 0x04); // typeName

                    output.Write(input.ReadBytes(0x10)); // Canvas rect
                }

                output.Write(input.ReadBytes(0x08)); // canvasScrollPosition
                output.Write(input.ReadBytes(0x04)); // canvasZoom
            }

            var remainderLength = (int) (input.Length - input.Position);
            var remainder = input.ReadBytes(remainderLength);
            output.Write(remainder);
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
