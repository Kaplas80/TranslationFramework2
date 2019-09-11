using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TF.Core.Files;
using TF.Core.TranslationEntities;
using TF.IO;
using UnderRailLib;
using UnderRailLib.AssemblyResolver;
using UnderRailLib.Models;

namespace TFGame.UnderRail.Files.Udlg
{
    public class File : BinaryTextFile
    {
        private DialogModel _model;

        public File(string path, string changesFolder, System.Text.Encoding encoding) : base(path, changesFolder, encoding)
        {
        }

        protected override IList<Subtitle> GetSubtitles()
        {
            var assemblyResolver = new AssemblyResolver();
            assemblyResolver.Initialize();
            Binder.SetAssemblyResolver(assemblyResolver);

            var dialogManager = new DialogManager();

            _model = dialogManager.LoadModel(Path);

            var dictionary = GetSubtitles(_model);

            var result = new List<Subtitle>(dictionary.Count);
            foreach (var pair in dictionary)
            {
                var sub = new UdlgSubtitle
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
                    var sub = subtitle as UdlgSubtitle;
                    output.WriteString(sub.Id);
                    output.WriteString(subtitle.Translation);

                    subtitle.Loaded = subtitle.Translation;
                }
            }

            NeedSaving = false;
            OnFileChanged();
        }

        protected virtual void LoadChanges(IList<Subtitle> subtitles)
        {
            if (HasChanges)
            {
                var subs = subtitles.Select(subtitle => subtitle as UdlgSubtitle).ToList();
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

                        if (!result.ContainsKey(question.Name))
                        {
                            result.Add(question.Name, dict["English"]);
                        }

                        foreach (var answer in question.PossibleAnswers)
                        {
                            if (!processed.ContainsKey(answer.Name))
                            {
                                queue.Enqueue(answer);
                            }
                        }

                        break;
                    }
                    case StoryElement storyElement:
                    {
                        var dict = storyElement.LocalizedTexts;

                        if (!result.ContainsKey(storyElement.Name))
                        {
                            result.Add(storyElement.Name, dict["English"]);
                        }

                        break;
                    }
                }

                foreach (var step in current.PossibleNextSteps)
                {
                    if (!processed.ContainsKey(step.Name))
                    {
                        queue.Enqueue(step);
                    }
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

            var dictionary = subtitles.Select(subtitle => subtitle as UdlgSubtitle).ToDictionary(udlgSubtitle => udlgSubtitle.Id, udlgSubtitle => udlgSubtitle.Translation);

            SetSubtitles(_model, dictionary);

            var assemblyResolver = new AssemblyResolver();
            assemblyResolver.Initialize();
            Binder.SetAssemblyResolver(assemblyResolver);

            var dialogManager = new DialogManager();

            dialogManager.SaveModel(_model, outputPath);
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
                            dict["English"] = dictionary[question.Name];
                        }

                        foreach (var answer in question.PossibleAnswers)
                        {
                            if (!processed.ContainsKey(answer.Name))
                            {
                                queue.Enqueue(answer);
                            }
                        }

                        break;
                    }
                    case StoryElement storyElement:
                    {
                        var dict = storyElement.LocalizedTexts;

                        if (dictionary.ContainsKey(storyElement.Name))
                        {
                            dict["English"] = dictionary[storyElement.Name];
                        }

                        break;
                    }
                }

                foreach (var step in current.PossibleNextSteps)
                {
                    if (!processed.ContainsKey(step.Name))
                    {
                        queue.Enqueue(step);
                    }
                }

                processed.Add(current.Name, true);
            }
        }
    }
}
