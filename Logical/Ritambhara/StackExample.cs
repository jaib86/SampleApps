using System;
using System.Collections.Generic;
using System.Text;

namespace Ritambhara
{
    /// <summary>
    /// http://www.ritambhara.in
    /// </summary>
    public static class StackExample
    {
        /// <summary>
        /// Question asked in one of the interview for a product based company.
        /// <para>http://www.ritambhara.in/check-duplicate-parenthesis-in-an-expression/</para>
        /// <para> Valid Patterns: {}[](), [{({[]})}], [[({})]], [[{{((())}}]]</para>
        /// <para> Invalid Patterns: [{(()}], [{())}], []{}((), []{}()), {{((}})), }{</para>
        /// </summary>
        /// <param name="pattern">string input pattern should contain any brackets in any combination</param>
        public static bool IsValidBracketsPatterns(string pattern)
        {
            Dictionary<char, char> bracketPairs = new Dictionary<char, char>
            {
                ['('] = ')',
                ['{'] = '}',
                ['['] = ']'
            };

            Stack<char> stack = new Stack<char>();

            foreach (char bracket in pattern)
            {
                if (bracketPairs.ContainsKey(bracket))
                {
                    stack.Push(bracket);
                }
                else if (bracketPairs.ContainsValue(bracket))
                {
                    char startBracket;

                    try
                    {
                        startBracket = stack.Pop();
                    }
                    catch (InvalidOperationException)
                    {
                        return false;
                    }

                    if (bracketPairs[startBracket] != bracket)
                    {
                        return false;
                    }
                }
            }

            return stack.Count == 0;
        }

        /// <summary>
        /// Reverse a string.
        /// For e.g. "There was a law" to "law a was There"
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string GetReverseString(string text)
        {
            Stack<string> words = new Stack<string>();
            List<char> word = new List<char>();

            foreach (var ch in text)
            {
                if (ch == ' ')
                {
                    words.Push(new string(word.ToArray()));
                    word.Clear();
                }
                else
                {
                    word.Add(ch);
                }
            }

            if (word.Count > 1)
            {
                words.Push(new string(word.ToArray()));
            }

            var result = string.Join(" ", words);

            return result;
        }
    }
}
