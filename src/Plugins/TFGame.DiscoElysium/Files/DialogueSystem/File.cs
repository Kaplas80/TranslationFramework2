namespace TFGame.DiscoElysium.Files.DialogueSystem
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using TF.Core.Files;
    using TF.Core.TranslationEntities;
    using TF.IO;
    using TFGame.DiscoElysium.Files.Common;
    using WeifenLuo.WinFormsUI.Docking;

    public class File : BinaryTextFile
    {
        public File(string gameName, string path, string changesFolder, System.Text.Encoding encoding) : base(gameName, path, changesFolder, encoding)
        {
        }

        public override void Open(DockPanel panel)
        {
            _view = new GridView(this);

            _subtitles = GetSubtitles();
            _view.LoadData(_subtitles.Where(x => !string.IsNullOrEmpty(x.Text)).ToList());
            _view.Show(panel, DockState.Document);
        }

        protected override IList<Subtitle> GetSubtitles()
        {
            var result = new List<Subtitle>();
            using (var fs = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fs, FileEncoding))
            {
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

                    if (fields.ContainsKey("Name") && fields.ContainsKey("Technical Name"))
                    {
                        string name = fields["Name"].Value;
                        string technicalName = fields["Technical Name"].Value;

                        if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(technicalName))
                        {
                            var subtitle = new DiscoElysiumSubtitle
                            {
                                Id = $"Actor_{technicalName}",
                                Offset = 0,
                                Text = name,
                                Loaded = name,
                                Translation = name
                            };

                            subtitle.PropertyChanged += SubtitlePropertyChanged;
                            result.Add(subtitle);
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

                    if (fields.ContainsKey("Name"))
                    {
                        string name = fields["Name"].Value;
                        var translatableFields = new string[] { "Description", "displayname", "description", "fixtureBonus", "requirement", "bonus", "fixtureDescription" };
                        foreach (string translatableField in translatableFields)
                        {
                            if (fields.TryGetValue(translatableField, out Field field))
                            {
                                if (!string.IsNullOrEmpty(field.Value))
                                {
                                    var subtitle = new DiscoElysiumSubtitle
                                    {
                                        Id = $"Item_{name}_{translatableField}",
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

                    if (fields.ContainsKey("Name") && fields.ContainsKey("Technical Name"))
                    {
                        string name = fields["Name"].Value;
                        string technicalName = fields["Technical Name"].Value;

                        if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(technicalName))
                        {
                            var subtitle = new DiscoElysiumSubtitle
                            {
                                Id = $"Location_{technicalName}",
                                Offset = 0,
                                Text = name,
                                Loaded = name,
                                Translation = name
                            };

                            subtitle.PropertyChanged += SubtitlePropertyChanged;
                            result.Add(subtitle);
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

                    if (fields.ContainsKey("Name") && fields.ContainsKey("Description"))
                    {
                        string name = fields["Name"].Value;
                        string desc = fields["Description"].Value;

                        if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(desc))
                        {
                            var subtitle = new DiscoElysiumSubtitle
                            {
                                Id = $"Variable_{name}",
                                Offset = 0,
                                Text = desc,
                                Loaded = desc,
                                Translation = desc
                            };

                            subtitle.PropertyChanged += SubtitlePropertyChanged;
                            result.Add(subtitle);
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

                        // TODO: Comprobar si hay que traducir el "Name"
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

                        if (fields2.ContainsKey("Title") && fields2.ContainsKey("Dialogue Text"))
                        {
                            string name = fields2["Title"].Value;
                            string desc = fields2["Dialogue Text"].Value;

                            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(desc))
                            {
                                var subtitle = new DiscoElysiumSubtitle
                                {
                                    Id = $"DialogueEntry_{name}_{id}_{dialogueEntryId}",
                                    Offset = 0,
                                    Text = desc,
                                    Loaded = desc,
                                    Translation = desc
                                };

                                subtitle.PropertyChanged += SubtitlePropertyChanged;
                                result.Add(subtitle);
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
            }

            LoadChanges(result);

            return result;
        }

        public override void SaveChanges()
        {
            using (var fs = new FileStream(ChangesFile, FileMode.Create))
            using (var output = new ExtendedBinaryWriter(fs, System.Text.Encoding.Unicode))
            {
                output.Write(ChangesFileVersion);
                output.Write(_subtitles.Count);
                foreach (var subtitle in _subtitles)
                {
                    var sub = subtitle as DiscoElysiumSubtitle;
                    output.WriteString(sub.Id);
                    output.WriteString(subtitle.Translation);

                    subtitle.Loaded = subtitle.Translation;
                }
            }

            NeedSaving = false;
            OnFileChanged();
        }

        protected override void LoadChanges(IList<Subtitle> subtitles)
        {
            if (HasChanges)
            {
                var subs = subtitles.Select(subtitle => subtitle as DiscoElysiumSubtitle).ToList();
                var dictionary = new Dictionary<string, DiscoElysiumSubtitle>(subs.Count);
                foreach (DiscoElysiumSubtitle subtitle in subs)
                {
                    dictionary.Add(subtitle.Id, subtitle);
                }

                using (var fs = new FileStream(ChangesFile, FileMode.Open))
                using (var input = new ExtendedBinaryReader(fs, System.Text.Encoding.Unicode))
                {
                    var version = input.ReadInt32();

                    if (version != ChangesFileVersion)
                    {
                        //File.Delete(ChangesFile);
                        return;
                    }

                    var subtitleCount = input.ReadInt32();

                    for (var i = 0; i < subtitleCount; i++)
                    {
                        var id = input.ReadString();
                        var text = input.ReadString();

                        if (dictionary.TryGetValue(id, out DiscoElysiumSubtitle subtitle))
                        {
                            subtitle.PropertyChanged -= SubtitlePropertyChanged;
                            subtitle.Translation = text;
                            subtitle.Loaded = subtitle.Translation;
                            subtitle.PropertyChanged += SubtitlePropertyChanged;
                        }
                    }
                }
            }
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

                    string technicalName = fields["Technical Name"].Value;

                    foreach (var kvp in fields)
                    {
                        output.WriteStringSerialized(kvp.Value.Title, 0x04);
                        if (kvp.Value.Title == "Name")
                        {
                            var subtitle = dictionary[$"Actor_{technicalName}"];
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
                    
                    string name = fields["Name"].Value;
                    var translatableFields = new string[] { "Description", "displayname", "description", "fixtureBonus", "requirement", "bonus", "fixtureDescription" };
                    foreach (var kvp in fields)
                    {
                        output.WriteStringSerialized(kvp.Value.Title, 0x04);
                        if (translatableFields.Contains(kvp.Value.Title) && !string.IsNullOrEmpty(kvp.Value.Value))
                        {
                            var key = $"Item_{name}_{kvp.Value.Title}";
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

                    string technicalName = fields["Technical Name"].Value;

                    foreach (var kvp in fields)
                    {
                        output.WriteStringSerialized(kvp.Value.Title, 0x04);
                        if (kvp.Value.Title == "Name")
                        {
                            var subtitle = dictionary[$"Location_{technicalName}"];
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

                    string name = fields["Name"].Value;

                    foreach (var kvp in fields)
                    {
                        output.WriteStringSerialized(kvp.Value.Title, 0x04);
                        if (kvp.Value.Title == "Description")
                        {
                            var subtitle = dictionary[$"Variable_{name}"];
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
                        output.WriteStringSerialized(field.Title, 0x04);
                        field.Value = input.ReadStringSerialized(0x04);
                        output.WriteStringSerialized(field.Value, 0x04);
                        field.Type = input.ReadInt32();
                        output.Write(field.Type);
                        field.TypeString = input.ReadStringSerialized(0x04);
                        output.WriteStringSerialized(field.TypeString, 0x04);

                        fields.Add(field.Title, field);

                        // TODO: Comprobar si hay que traducir el "Name"
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

                        string name = fields2[$"Title"].Value;

                        foreach (var kvp in fields2)
                        {
                            output.WriteStringSerialized(kvp.Value.Title, 0x04);
                            if (kvp.Value.Title == "Dialogue Text" && !string.IsNullOrEmpty(kvp.Value.Value))
                            {
                                var subtitle = dictionary[$"DialogueEntry_{name}_{id}_{dialogueEntryId}"];
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

                var remainderLength = (int)(input.Length - input.Position);
                var remainder = input.ReadBytes(remainderLength);
                output.Write(remainder);
            }
        }

        protected override string GetContext(Subtitle subtitle)
        {
            return (subtitle as DiscoElysiumSubtitle).Id.Replace(LineEnding.ShownLineEnding, LineEnding.PoLineEnding);
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
