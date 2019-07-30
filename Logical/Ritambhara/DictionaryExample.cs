using System;
using System.Collections.Generic;

namespace Ritambhara
{
    public static class DictionaryExample
    {
        public static Dictionary<char, int> GetFrequencyOfChar(string value)
        {
            Dictionary<char, int> charsCount = new Dictionary<char, int>();

            foreach (char ch in value)
            {
                if (charsCount.ContainsKey(ch))
                    charsCount[ch]++;
                else
                    charsCount[ch] = 1;
            }

            return charsCount;
        }
    }
}
