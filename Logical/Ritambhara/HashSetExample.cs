using System;
using System.Collections.Generic;
using System.Text;

namespace Ritambhara
{
    public static class HashSetExample
    {
        public static char FindFirstDuplicateChar(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                HashSet<char> uniqueChars = new HashSet<char>();

                foreach (var ch in value)
                {
                    if (uniqueChars.Contains(ch))
                    {
                        return ch;
                    }
                    else
                    {
                        uniqueChars.Add(ch);
                    }
                }
            }

            return default(char);
        }
    }
}
