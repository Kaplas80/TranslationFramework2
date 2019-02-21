using System;

namespace TF.Core.Helpers
{
    public static class SearchHelper
    {
        public static unsafe int SearchPattern(byte[] searchArray, byte[] pattern, int startIndex = 0)
        {
            // https://gist.github.com/mjs3339/0772431281093f1bca1fce2f2eca527d
            var patternLength = pattern.Length;
            if (pattern == null)
            {
                throw new Exception("Pattern has not been set.");
            }

            if (patternLength > searchArray.Length)
            {
                throw new Exception("Search Pattern length exceeds search array length.");
            }

            var jumpTable = new int[256];
            for (var i = 0; i < 256; i++)
            {
                jumpTable[i] = patternLength;
            }

            for (var i = 0; i < patternLength - 1; i++)
            {
                jumpTable[pattern[i]] = patternLength - i - 1;
            }

            var index = startIndex;
            var limit = searchArray.Length - patternLength;
            var patternLengthMinusOne = patternLength - 1;
            fixed (byte* pointerToByteArray = searchArray)
            {
                var pointerToByteArrayStartingIndex = pointerToByteArray + startIndex;
                fixed (byte* pointerToPattern = pattern)
                {
                    while (index <= limit)
                    {
                        var j = patternLengthMinusOne;
                        while (j >= 0 && pointerToPattern[j] == pointerToByteArrayStartingIndex[index + j])
                        {
                            j--;
                        }

                        if (j < 0)
                        {
                            return index;
                        }

                        index += Math.Max(jumpTable[pointerToByteArrayStartingIndex[index + j]] - patternLength + 1 + j, 1);
                    }
                }
            }

            return -1;
        }
    }
}
