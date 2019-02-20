using System;
using System.Collections.Generic;

namespace TF.Core.Entities
{
    public class TranslationFileContainer
    {
        public string Id { get; }
        public string Path { get; }
        public ContainerType Type { get; }
        public IList<TranslationFile> Files { get; }

        public TranslationFileContainer(string path, ContainerType type)
        {
            Id = Guid.NewGuid().ToString();
            Path = path;
            Type = type;
            Files = new List<TranslationFile>();
        }

        public TranslationFileContainer(string id, string path, ContainerType type)
        {
            Id = id;
            Path = path;
            Type = type;
            Files = new List<TranslationFile>();
        }

        public void AddFile(TranslationFile file)
        {
            Files.Add(file);
        }

        public void Restore()
        {
            foreach (var file in Files)
            {
                file.Restore();
            }
        }

        public override string ToString()
        {
            return Path;
        }
    }
}
