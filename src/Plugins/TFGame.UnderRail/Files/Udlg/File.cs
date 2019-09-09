using System;
using System.Collections.Generic;
using System.IO;
using TF.Core.Files;
using TF.Core.TranslationEntities;
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
            var result = new List<Subtitle>();
            
            var assemblyResolver = new AssemblyResolver();
            assemblyResolver.Initialize();
            var eventHandler = new ResolveEventHandler(assemblyResolver.ResolveAssembly);
            AppDomain.CurrentDomain.AssemblyResolve += eventHandler;
            Binder.SetAssemblyResolver(assemblyResolver);

            var dialogManager = new DialogManager();

            _model = dialogManager.LoadModel(Path);

            result = GetSubtitles(_model);

            result.Sort();
            LoadChanges(result);

            return result;
        }

        private static List<Subtitle> GetSubtitles(DialogModel model)
        {
            var result = new List<Subtitle>();
            result.AddRange(GetSubtitles(model.StartElement));
            return result;
        }

        static List<UdlgSubtitle> GetSubtitles(ConditionalElement element)
        {
            var result = new List<UdlgSubtitle>();

            foreach (var step in element.PossibleNextSteps)
            {
                switch (step)
                {
                    case Question question:
                        result.AddRange(GetSubtitles(question));
                        break;
                    case StoryElement storyElement:
                        result.AddRange(GetSubtitles(storyElement));
                        break;
                }
            }

            return result;
        }

        private static IList<UdlgSubtitle> GetSubtitles(StoryElement element)
        {
            var result = new List<UdlgSubtitle>();
            var dict = element.LocalizedTexts;

            var sub = new UdlgSubtitle
            {
                Id = element.Name,
                Text = dict["English"],
                Loaded = dict["English"],
                Translation = dict["English"],
                Offset = -1
            };

            result.Add(sub);

            result.AddRange(GetSubtitles(element as ConditionalElement));
            return result;
        }

        private static IList<UdlgSubtitle> GetSubtitles(Question element)
        {
            var result = new List<UdlgSubtitle>();
            var dict = element.LocalizedTexts;

            var sub = new UdlgSubtitle
            {
                Id = element.Name,
                Text = dict["English"],
                Loaded = dict["English"],
                Translation = dict["English"],
                Offset = -1
            };

            result.Add(sub);

            foreach (var answer in element.PossibleAnswers)
            {
                result.AddRange(GetSubtitles(answer));
            }
            result.AddRange(GetSubtitles(element as ConditionalElement));
            return result;
        }

        public override void Rebuild(string outputFolder)
        {
            var outputPath = System.IO.Path.Combine(outputFolder, RelativePath);
            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(outputPath));

            var subtitles = GetSubtitles();

            
        }
    }
}
