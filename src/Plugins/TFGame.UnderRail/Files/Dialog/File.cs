using System.Collections.Generic;
using System.IO;
using System.Linq;
using TF.Core.Files;
using TF.Core.TranslationEntities;
using TF.IO;
using TFGame.UnderRail.Files.Common;
using UnderRailLib;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models;
using WeifenLuo.WinFormsUI.Docking;

namespace TFGame.UnderRail.Files.Dialog
{
    public class File : BinaryTextFile
    {
        private DialogModel _model;

        public override string LineEnding => "\r\n";

        public File(string path, string changesFolder, System.Text.Encoding encoding) : base(path, changesFolder, encoding)
        {
        }

        public override void Open(DockPanel panel)
        {
            _view = new GridView(LineEnding);

            _subtitles = GetSubtitles();
            _view.LoadData(_subtitles.Where(x => !string.IsNullOrEmpty(x.Text)).ToList());
            _view.Show(panel, DockState.Document);
        }

        protected override IList<Subtitle> GetSubtitles()
        {
            var assemblyResolver = new AssemblyResolver();
            assemblyResolver.Initialize();
            Binder.SetAssemblyResolver(assemblyResolver);

            var fileManager = new FileManager<DialogModel>();

            _model = fileManager.Load(Path, false);

            var dictionary = GetSubtitles(_model);

            var result = new List<Subtitle>(dictionary.Count);
            foreach (var pair in dictionary)
            {
                var sub = new UnderRailSubtitle
                {
                    Id = pair.Key,
                    Text = pair.Value,
                    Loaded = pair.Value,
                    Translation = pair.Value,
                    Offset = 0,
                };
                sub.PropertyChanged += SubtitlePropertyChanged;

                result.Add(sub);
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
                    var sub = subtitle as UnderRailSubtitle;
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
                var subs = subtitles.Select(subtitle => subtitle as UnderRailSubtitle).ToList();
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

                        var subtitle = subs.FirstOrDefault(x => x.Id == id);
                        if (subtitle != null)
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

        private Dictionary<string, string> GetSubtitles(DialogModel model)
        {
            var result = new Dictionary<string, string>();

            if (model == null)
            {
                return result;
            }

            var processed = new Dictionary<string, bool>();

            var queue = new Queue<ConditionalElement>();
            queue.Enqueue(model.StartElement);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                if (processed.ContainsKey(current.Name))
                {
                    continue;
                }

                switch (current)
                {
                    case Question question:
                    {
                        var dict = question.LocalizedTexts;

                        if (dict.Count > 0 && !result.ContainsKey(question.Name))
                        {
                            var str = FileEncoding.GetString(FileEncoding.GetBytes(dict["English"]));
                            result.Add(question.Name, str);
                        }

                        foreach (var answer in question.PossibleAnswers)
                        {
                            queue.Enqueue(answer);
                        }

                        break;
                    }
                    case StoryElement storyElement:
                    {
                        var dict = storyElement.LocalizedTexts;

                        if (dict.Count > 0 && !result.ContainsKey(storyElement.Name))
                        {
                            var str = FileEncoding.GetString(FileEncoding.GetBytes(dict["English"]));
                            result.Add(storyElement.Name, str);
                        }

                        break;
                    }
                }

                foreach (var step in current.PossibleNextSteps)
                {
                    queue.Enqueue(step);
                }

                processed.Add(current.Name, true);
            }

            return result;
        }

        

        public override void Rebuild(string outputFolder)
        {
            var outputPath = System.IO.Path.Combine(outputFolder, RelativePath);
            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(outputPath));

            var subtitles = GetSubtitles();

            var dictionary = subtitles.Select(subtitle => subtitle as UnderRailSubtitle).ToDictionary(udlgSubtitle => udlgSubtitle.Id, udlgSubtitle => udlgSubtitle.Translation);

            SetSubtitles(_model, dictionary);

            var assemblyResolver = new AssemblyResolver();
            assemblyResolver.Initialize();
            Binder.SetAssemblyResolver(assemblyResolver);

            var fileManager = new FileManager<DialogModel>();

            fileManager.Save(_model, outputPath, false);
        }

        private void SetSubtitles(DialogModel model, Dictionary<string, string> dictionary)
        {
            var processed = new Dictionary<string, bool>();
            var queue = new Queue<ConditionalElement>();
            queue.Enqueue(model.StartElement);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                if (processed.ContainsKey(current.Name))
                {
                    continue;
                }

                switch (current)
                {
                    case Question question:
                    {
                        var dict = question.LocalizedTexts;

                        if (dictionary.ContainsKey(question.Name))
                        {
                            var str = FileEncoding.GetString(FileEncoding.GetBytes(dictionary[question.Name]));
                            dict["English"] = str;
                        }

                        foreach (var answer in question.PossibleAnswers)
                        {
                            queue.Enqueue(answer);
                        }

                        break;
                    }
                    case StoryElement storyElement:
                    {
                        var dict = storyElement.LocalizedTexts;

                        if (dictionary.ContainsKey(storyElement.Name))
                        {
                            var str = FileEncoding.GetString(FileEncoding.GetBytes(dictionary[storyElement.Name]));
                            dict["English"] = str;
                        }

                        break;
                    }
                }

                foreach (var step in current.PossibleNextSteps)
                {
                    queue.Enqueue(step);
                }

                processed.Add(current.Name, true);
            }
        }
    }
}
