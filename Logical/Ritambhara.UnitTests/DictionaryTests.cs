using System.Collections.Generic;
using Xunit;

namespace Ritambhara.UnitTests
{
    public class DictionaryTests
    {
        [Fact]
        public void GetFrequencyOfChar1()
        {
            Dictionary<char, int> charWiseCount = DictionaryExample.GetFrequencyOfChar("Hello World");

            Assert.Equal(8, charWiseCount.Count);
            Assert.Equal(1, charWiseCount['H']);
            Assert.Equal(1, charWiseCount['e']);
            Assert.Equal(3, charWiseCount['l']);
            Assert.Equal(2, charWiseCount['o']);
            Assert.Equal(1, charWiseCount[' ']);
            Assert.Equal(1, charWiseCount['W']);
            Assert.Equal(1, charWiseCount['r']);
            Assert.Equal(1, charWiseCount['d']);
        }

        [Fact]
        public void GetFrequencyOfChar2()
        {
            Dictionary<char, int> charWiseCount = DictionaryExample.GetFrequencyOfChar("Jaiprakash");

            Assert.Equal(8, charWiseCount.Count);
            Assert.Equal(1, charWiseCount['J']);
            Assert.Equal(3, charWiseCount['a']);
            Assert.Equal(1, charWiseCount['i']);
            Assert.Equal(1, charWiseCount['p']);
            Assert.Equal(1, charWiseCount['r']);
            Assert.Equal(1, charWiseCount['k']);
            Assert.Equal(1, charWiseCount['s']);
            Assert.Equal(1, charWiseCount['h']);
        }

        [Fact]
        public void GetFrequencyOfChar3()
        {
            Dictionary<char, int> charWiseCount = DictionaryExample.GetFrequencyOfChar("Jaiprakash Barnwal");

            Assert.Equal(13, charWiseCount.Count);
            Assert.Equal(1, charWiseCount['J']);
            Assert.Equal(5, charWiseCount['a']);
            Assert.Equal(1, charWiseCount['i']);
            Assert.Equal(1, charWiseCount['p']);
            Assert.Equal(2, charWiseCount['r']);
            Assert.Equal(1, charWiseCount['k']);
            Assert.Equal(1, charWiseCount['s']);
            Assert.Equal(1, charWiseCount['h']);
            Assert.Equal(1, charWiseCount[' ']);
            Assert.Equal(1, charWiseCount['B']);
            Assert.Equal(1, charWiseCount['n']);
            Assert.Equal(1, charWiseCount['w']);
            Assert.Equal(1, charWiseCount['l']);
        }

        [Fact]
        public void GetFrequencyOfChar4()
        {
            Dictionary<char, int> charWiseCount = DictionaryExample.GetFrequencyOfChar("NAGARRO");

            Assert.Equal(5, charWiseCount.Count);
            Assert.Equal(1, charWiseCount['N']);
            Assert.Equal(2, charWiseCount['A']);
            Assert.Equal(1, charWiseCount['G']);
            Assert.Equal(2, charWiseCount['R']);
            Assert.Equal(1, charWiseCount['O']);
        }
    }
}