using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace TF.Core.Entities
{
    public class TranslationFileContainer
    {
        public string Id { get; }
        public string Path { get; }
        public ContainerType Type { get; }
        public ConcurrentBag<TranslationFile> Files { get; }

        public TranslationFileContainer(string path, ContainerType type)
        {
            Id = Guid.NewGuid().ToString();
            Path = path;
            Type = type;
            Files = new ConcurrentBag<TranslationFile>();
        }

        public TranslationFileContainer(string id, string path, ContainerType type)
        {
            Id = id;
            Path = path;
            Type = type;
            Files = new ConcurrentBag<TranslationFile>();
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
