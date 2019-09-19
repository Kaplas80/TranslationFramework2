using System.Collections.Generic;

namespace ParLib
{
    public sealed class LowerCaseComparer : IComparer<string>
    {
        public int Compare(string a, string b)
        {
            return string.CompareOrdinal(a.ToLowerInvariant(), b.ToLowerInvariant());
        }
    }
}
