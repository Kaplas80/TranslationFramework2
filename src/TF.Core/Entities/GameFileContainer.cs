using System;
using System.Collections.Generic;

namespace TF.Core.Entities
{
    public enum ContainerType
    {
        Folder,
        CompressedFile
    }

    public class GameFileContainer : IComparable<GameFileContainer>
    {
        public string Path { get; set; }
        public ContainerType Type { get; set; }
        public IList<GameFileSearch> FileSearches { get; set; }

        public GameFileContainer()
        {
            FileSearches = new List<GameFileSearch>();
        }

        public int CompareTo(GameFileContainer other)
        {
            return string.Compare(Path, other.Path, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}